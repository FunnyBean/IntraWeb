using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System;
using Krosbook.Filters;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Krosbook.Models.Cars;
using Krosbook.ViewModels.Cars;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Krosbook.Models.Reservation;

namespace Krosbook.Controllers.Api.v1
{
    [Route("api/cars")]
    [EnableCors("AllowAll")]
    [Authorize]
    public class CarsController : BaseController
    {
        #region Private Fields

        private ICarRepository _carRepository;
        private ICarReservationRepository _carReservationRepository;
        private ILogger<CarsController> _logger;
        private IMapper _mapper;

        #endregion

        #region Constructor
        public CarsController(
            ICarRepository carRepository,
            ICarReservationRepository carReservationRepository,
            ILogger<CarsController> logger,
            IMapper mapper)
        {
            _carRepository = carRepository;
            _logger = logger;
            _mapper = mapper;
            _carReservationRepository = carReservationRepository;
        }

        #endregion


        #region API
        [HttpGet]
        public IEnumerable<CarViewModel> GetAllCars()
        {
            return _mapper.Map<IEnumerable<CarViewModel>>(_carRepository.GetAll());
        }


        [HttpPost()]
        [Authorize(Roles = "Admin")]
        [ValidateModelState, CheckArgumentsForNull]
        public IActionResult PostNewCar([FromBody] CarViewModel carVm)
        {
            return this.CreateNewCar(carVm);
        }



        [HttpGet("{carId}")]
        public IActionResult GetCarById(int carId)
        {
            var car = _carRepository.GetItem(carId);
            if (car == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.Json(null);
            }
            else
            {
                return this.Json(_mapper.Map<CarViewModel>(car));
            }
        }



        [HttpPut("{carId}")]
        [Authorize(Roles = "Admin")]
        [ValidateModelState, CheckArgumentsForNull]
        public IActionResult UpdateCar(int carId, [FromBody] CarViewModel carVm)
        {
            if (carVm.Id != carId)
            {
                var message = $"Invalid argument. Id '{carId}' and userVm.Id '{carVm.Id}' are not equal.";
                _logger.LogWarning(message);
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json(new { Message = message });
            }

            Car oldCar = _carRepository.GetItem(carId);
            if (oldCar == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.Json(null);
            }

            if (ExistAnotherUserWithPlate(carVm.Plate, carId))
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json(new { Message = $"Car with plate '{carVm.Plate}' already exist." });
            }
            else
            {
                IActionResult result;
                Car editedCar = _mapper.Map(carVm, oldCar);
                result = SaveData(() =>
                {
                    _carRepository.Edit(editedCar);
                });
                return result;
            }
        }



        [HttpDelete("{carId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCar(int carId)
        {
            return SaveData(() =>
            {
                _carRepository.Delete(carId);
            });
        }


        [HttpPost("filter")]
        public IEnumerable<CarViewModel> GetAllUnreservedCars([FromBody] CarReservationViewModel reservationVM)
        {
            IQueryable<Car> cars = _carRepository.GetAll();
            var exp = new List<Car>();
            foreach (var car in cars)
            {
                if (CheckUnreservedCar(car.Id, DateTime.Parse(reservationVM.date), reservationVM.length))
                {
                    exp.Add(car);
                }
            }
            return _mapper.Map<IEnumerable<CarViewModel>>(exp);
        }

        #endregion




        #region Helpers

        private IActionResult CreateNewCar(CarViewModel carVm)
        {
            if (_carRepository.GetItem(u => u.Plate == carVm.Plate) == null)
            {
                Car car = _mapper.Map<Car>(carVm);
                return SaveData(() =>
                {
                    _carRepository.Add(car);
                },
                () =>
                {
                    this.Response.StatusCode = (int)HttpStatusCode.Created;
                    return this.Json(new JsonResult(this.Json(_mapper.Map<CarViewModel>(car)))
                    {
                        StatusCode = this.Response.StatusCode
                    });
                });
            }
            else
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json(new JsonResult($"Car with plate '{carVm.Plate}' already exist.")
                {
                    StatusCode = this.Response.StatusCode
                });
            }
        }



        public bool CheckUnreservedCar(int carId, DateTime date, int length)
        {
            IQueryable<CarReservation> reservations = _carReservationRepository.Get(r => r.CarId == carId && r.dateTime.Date == date.Date); //all reservations my car of current day 
            var isFree = true;
            foreach (var res in reservations)
            {
                var dat = date;
                //go through reservations, add their length and check if isn't any time duplication
                for (var a = length; a >= 0; a -= 30)
                {
                    if (res.dateTime.TimeOfDay != dat.TimeOfDay)
                    {
                        var reservationTime = res.dateTime;
                        for (var i = res.length; i > 0; i -= 30)
                        {

                            if (reservationTime.TimeOfDay == dat.TimeOfDay)
                            {
                                isFree = false;
                                break;
                            }
                            reservationTime = reservationTime.AddMinutes(30);
                        }
                    }
                    else
                    {
                        if (a == 0)
                        {
                            isFree = true;
                            break;
                        }
                        else
                        {
                            isFree = false;
                            break;
                        }
                    }

                    dat = dat.AddMinutes(30);
                }
            }
            return isFree;
        }







        private bool ExistAnotherUserWithPlate(string plate, int carId)
        {
            Car car = _carRepository.GetItem(u => u.Plate == plate);
            return car != null && car.Id != carId;
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
                _carRepository.Save();
                return result();
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured when saving data.", ex);
                this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return this.Json(new { Message = $"Saving data throw Exception '{ex.Message}'" });
            }
        }

        #endregion
    }
}
