using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System;
using Krosbook.Filters;
using Microsoft.Extensions.Logging;
using Krosbook.Models.Users;
using Krosbook.ViewModels.Users;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Krosbook.Models.Cars;
using Krosbook.ViewModels.Cars;
using Krosbook.Models.Reservation;
using Krosbook.ViewModels.Rooms;
using Krosbook.ViewModels.Reservation;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Krosbook.Controllers.Api.v1
{
    [Route("api/reservations/cars")]
    [EnableCors("AllowAll")]
    [Authorize]
    public class CarReservationController : BaseController
    {
        #region Private Fields
        private ICarReservationRepository _reservationRepository;
        private ILogger<CarReservationController> _logger;
        private IMapper _mapper;
        #endregion

        #region Constructor
        public CarReservationController(ICarReservationRepository reservationRepository,
                      ILogger<CarReservationController> logger,
                      IMapper mapper)
        {
            _reservationRepository = reservationRepository;
            _logger = logger;
            _mapper = mapper;
        }
        #endregion


        #region API
        [HttpGet]        
        public IEnumerable<CarReservationViewModel> GetAllCarReservations()
        {
            return _mapper.Map<IEnumerable<CarReservationViewModel>>(_reservationRepository.GetAll());
        }



        [HttpPost("byCar/{carId}")]
        public IEnumerable<CarReservationViewModel> GetReservationByCar([FromBody] CarReservationIntervalViewModel reservation, int carId)
        {
            return _mapper.Map<IEnumerable<CarReservationViewModel>>(_reservationRepository.GetReservationsByCarInTimeInterval(carId, DateTime.Parse(reservation.from), DateTime.Parse(reservation.to)));
        }



        [HttpPost()]
        [ValidateModelState, CheckArgumentsForNull]
        public IActionResult CreateNewCarReservation([FromBody] CarReservationViewModel reservationVm)
        {
            reservationVm.dateTime = DateTime.Parse(reservationVm.date);
            return this.CreateNewReservation(reservationVm);
        }



        [HttpGet("{reservationId}")]        
        public IActionResult GetReservationById(int reservationId)
        {
            var reservation = _reservationRepository.GetItem(reservationId);
            if (reservation == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.Json(null);
            }
            else
            {
                return this.Json(_mapper.Map<CarReservationViewModel>(reservation));
            }
        }




        [HttpPut("{reservationId}")]
        [ValidateModelState, CheckArgumentsForNull]    
        public IActionResult UpdateReservation(int reservationId, [FromBody] CarReservationViewModel reservationVm)
        {
            if (!User.IsInRole("Admin"))
            {
                if (reservationVm.UserId != GetUserId())
                {
                    var message = $"User with id " + GetUserId() + " can't update reservation, that was created by user with id " + reservationVm.UserId;
                    _logger.LogWarning(message);
                    this.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return this.Json(new { Message = message });
                }
            }
          
                if (reservationVm.Id != reservationId)
                {
                    var message = $"Invalid argument. Id '{reservationId}' and userVm.Id '{reservationVm.Id}' are not equal.";
                    _logger.LogWarning(message);

                    this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return this.Json(new { Message = message });
                }

                CarReservation oldReservation = _reservationRepository.GetItem(reservationId);
                if (oldReservation == null)
                {
                    this.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return this.Json(null);
                }
                IActionResult result;
                CarReservation editedReservation = _mapper.Map(reservationVm, oldReservation);
                result = SaveData(() =>
                {
                    _reservationRepository.Edit(editedReservation);
                });
                return result;
        }



        [HttpDelete("{reservationId}")]
        public IActionResult DeleteReservation(int reservationId)
        {
            if (!User.IsInRole("Admin"))
            {
                if (_reservationRepository.GetItem(reservationId).UserId != GetUserId())
                {
                    var message = $"User with id " + GetUserId() + " can't delete reservation, that was created by user with id " + _reservationRepository.GetItem(reservationId).UserId;
                    _logger.LogWarning(message);
                    this.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return this.Json(new { Message = message });
                }
            }
            return SaveData(() =>
            {
                _reservationRepository.Delete(reservationId);
            });
        }

        #endregion




        #region Helpers

        private IActionResult CreateNewReservation(CarReservationViewModel reservationVm)
        {
            reservationVm.UserId = GetUserId();
            CarReservation reservation = _mapper.Map<CarReservation>(reservationVm);
            return SaveData(() =>
            {
                _reservationRepository.Add(reservation);
            },
            () =>
            {
                this.Response.StatusCode = (int)HttpStatusCode.Created;

                return this.Json(new JsonResult(this.Json(_mapper.Map<CarReservationViewModel>(reservation)))
                {
                    StatusCode = this.Response.StatusCode
                });
            });
        }



        private IActionResult SaveData(Action beforeAction)
        {
            return SaveData(beforeAction, () => this.Json(null));
        }

        private IActionResult SaveData(Action beforeAction, Func<IActionResult> result)
        {
            try
            {
                beforeAction();
                _reservationRepository.Save();

                return result();
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured when saving data.", ex);
                this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return this.Json(new { Message = $"Saving data throw Exception '{ex.Message}'" });
            }
        }


        public int GetUserId()
        {
            var claims = User.Claims.Select(claim => new { claim.Type, claim.Value}).ToArray();
            var userId = claims[0].Value;
            int id;
            int.TryParse(userId, out id);
            return id;
        }

        #endregion

    }
}
