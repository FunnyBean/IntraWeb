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
using Krosbook.Services.Email;
using Krosbook.Services.G2Meeting;

namespace Krosbook.Controllers.Api.v1
{
    [Route("api/reservations/rooms")]
    [EnableCors("AllowAll")]
    [Authorize]
    public class RoomReservationController : Controller
    {
        #region Private Fields

        private IRoomReservationRepository _reservationRepository;
        private ILogger<RoomReservationController> _logger;
        private IMapper _mapper;
        private IEmailService _emailService;
        private IG2MService _G2MService;


        #endregion

        #region Constructor
        public RoomReservationController(IRoomReservationRepository reservationRepository,
                      ILogger<RoomReservationController> logger,
                                       IMapper mapper, IEmailService emailService, IG2MService G2MService
                                       )
        {
            _reservationRepository = reservationRepository;
            _logger = logger;
            _mapper = mapper;
            _emailService = emailService;
            _G2MService = G2MService;
        }

        #endregion

        #region API

        [HttpGet]
        public IEnumerable<RoomReservationViewModel> GetAllRoomReservations()
        {
            return _mapper.Map<IEnumerable<RoomReservationViewModel>>(_reservationRepository.GetAll());
        }


        [HttpPost()]
        [ValidateModelState, CheckArgumentsForNull]
        public IActionResult CreateNewRoomReservation([FromBody] RoomReservationViewModel reservationVm)
        {
            reservationVm.dateTime = DateTime.Parse(reservationVm.date);
            this.CreateNewReservation(reservationVm);


            List<Models.Reservation.RoomReservation> reservation = _reservationRepository.Get(r => r.name == reservationVm.name && r.RoomId == reservationVm.RoomId && r.length == reservationVm.length && r.UserId == reservationVm.UserId && r.dateTime == reservationVm.dateTime).ToList();
            if (reservation[0] == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return this.Json(null);
            }
            else
            {
                this.Response.StatusCode = (int)HttpStatusCode.Created;
                return this.Json(_mapper.Map<RoomReservationViewModel>(reservation[0]));
            }
        }




        [HttpPost("byRoom/{roomId}")]
        public IEnumerable<RoomReservationViewModel> GetReservationByRoom([FromBody] RoomReservationIntervalViewModel reservation, int roomId)
        {
            return _mapper.Map<IEnumerable<RoomReservationViewModel>>(_reservationRepository.GetReservationsByRoomInTimeInterval(roomId, DateTime.Parse(reservation.from), DateTime.Parse(reservation.to)));
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
                return this.Json(_mapper.Map<RoomReservationViewModel>(reservation));
            }
        }



        [HttpPut("{reservationId}")]
        [ValidateModelState, CheckArgumentsForNull]
        public IActionResult UpdateReservation(int reservationId, [FromBody] RoomReservationViewModel reservationVm)
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

            Models.Reservation.RoomReservation oldReservation = _reservationRepository.GetItem(reservationId);
            if (oldReservation == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.Json(null);
            }

            IActionResult result;
            Models.Reservation.RoomReservation editedReservation = _mapper.Map(reservationVm, oldReservation);
            result = SaveData(() =>
            {

                if (reservationVm.goToMeeting)
                {
                    if (_G2MService.canCreateMeeting(reservationVm))
                    {

                        var meeting = new Citrix.GoToMeeting.Api.Model.MeetingCreated();
                        string joinUrl = "";
                        if (_reservationRepository.GetItem(editedReservation.Id).G2MeetingID == 0)
                        {
                            meeting = _G2MService.createNewMeeting(reservationVm);
                            joinUrl = meeting.joinURL;
                        }
                        else
                        {
                            var m = _G2MService.updateMeeting(reservationVm, _reservationRepository.GetItem(editedReservation.Id).G2MeetingID);
                            meeting = m;
                            joinUrl = m.joinURL;
                        }



                        editedReservation.G2MeetingID = int.Parse(meeting.meetingid.ToString());

                        if (reservationVm.emailInvitation)
                        {
                            _emailService.CreateEmailCalendarEvent(reservationVm, joinUrl);
                        }
                        else
                        {
                            _emailService.SendG2M(reservationVm, joinUrl);
                        }
                    }
                    else
                    {
                        this.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    }
                }
                else
                {
                    if (reservationVm.emailInvitation)
                    {
                        _emailService.CreateEmailCalendarEvent(reservationVm);
                    }
                }

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
                var g2mId = _reservationRepository.GetItem(reservationId).G2MeetingID;
                _reservationRepository.Delete(reservationId);
                if (g2mId != 0)
                {
                    _G2MService.deleteMeeting(g2mId);
                }

            });
        }


        #endregion

        
        #region Helpers

        private IActionResult CreateNewReservation(RoomReservationViewModel reservationVm)
        {
            reservationVm.UserId = GetUserId();
            Models.Reservation.RoomReservation reservation = _mapper.Map<Models.Reservation.RoomReservation>(reservationVm);

            return SaveData(() =>
            {
                _reservationRepository.Add(reservation);
            },
            () =>
            {
                this.Response.StatusCode = (int)HttpStatusCode.Created;

                return this.Json(new JsonResult(this.Json(_mapper.Map<RoomReservationViewModel>(reservation)))
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
            var claims = User.Claims.Select(claim => new { claim.Type, claim.Value }).ToArray();
            var userId = claims[0].Value;
            int id;
            int.TryParse(userId, out id);
            return id;
        }


        #endregion
    }
}
