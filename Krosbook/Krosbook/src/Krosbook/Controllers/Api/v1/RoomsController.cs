using System;
using System.Collections.Generic;
using System.Net;
using AutoMapper;
using Krosbook.Filters;
using Krosbook.Models.Rooms;
using Krosbook.ViewModels.Rooms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Krosbook.Models.Reservation;

namespace Krosbook.Controllers.Api.v1
{
    [Route("api/rooms")]
    [EnableCors("AllowAll")]
    [Authorize]
    public class RoomsController : BaseController
    {
        #region Private Fields

        private IRoomRepository _roomRepository;
        private IRoomReservationRepository _roomReservationRepository;
        private ILogger<RoomsController> _logger;
        private IMapper _mapper;

        #endregion

        #region Constructor
        public RoomsController(IRoomRepository roomRepository,
                      ILogger<RoomsController> logger,
                                       IMapper mapper,
                                       IRoomReservationRepository roomReservationRepository)
        {
            _roomRepository = roomRepository;
            _logger = logger;
            _mapper = mapper;
            _roomReservationRepository = roomReservationRepository;
        }
        #endregion

        #region API
        [HttpGet]
        public IEnumerable<RoomViewModel> GetAllRooms()
        {
            var rooms = _mapper.Map<IEnumerable<RoomViewModel>>(_roomRepository.GetAll());
            return rooms;
        }


        [HttpGet("{roomId}", Name = "GetRoom")]
        public IActionResult GetRoomById(int roomId)
        {
            var room = _roomRepository.GetItem(roomId);
            if (room == null)
            {
                this.Response.StatusCode = (int) HttpStatusCode.NotFound;
                return this.Json(null);
            }
            else
            {
                return this.Json(_mapper.Map<RoomViewModel>(room));
            }
        }



        [HttpGet("GetTypes")]
        public IEnumerable<string> GetAllTypes()
        {
            return _roomRepository.GetTypes();
        }



        [HttpPost()]
        [Authorize(Roles = "Admin")]
        [ValidateModelState, CheckArgumentsForNull]
        public IActionResult CreateNewRoom([FromBody] RoomViewModel roomVm)
        {
            if (_roomRepository.GetItem(roomVm.Name) == null)
            {
                var room = _mapper.Map<Room>(roomVm);
                return SaveData(() =>
                {
                    _roomRepository.Add(room);
                },
                () =>
                {
                    this.Response.StatusCode = (int) HttpStatusCode.Created;
                    return this.Json(_mapper.Map<RoomViewModel>(room));
                });
            }
            else
            {
                this.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return this.Json(new { Message = $"Room with name '{roomVm.Name}' already exist." });
            }
        }

       

        [HttpPut("{roomId}")]
        [Authorize(Roles = "Admin")]
        [ValidateModelState, CheckArgumentsForNull]
        public IActionResult UpdateRoom(int roomId, [FromBody] RoomViewModel roomVm)
        {
            if (roomVm.Id != roomId)
            {
                this.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                var message = $"Invalid argument. Id '{roomId}' and roomVm.Id '{roomVm.Id}' are not equal.";
                _logger.LogWarning(message);

                return this.Json(new { Message = message });
            }

            var editedRoom = _roomRepository.GetItem(roomId);
            if (editedRoom == null)
            {
                this.Response.StatusCode = (int) HttpStatusCode.NotFound;
                return this.Json(null);
            }

            if (ExistAnotherRoomWithName(roomVm.Name, roomId))
            {
                this.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return this.Json(new { Message = $"Room with name '{roomVm.Name}' already exist." });
            }
            else
            {
                editedRoom = _mapper.Map(roomVm, editedRoom);

                return SaveData(() =>
                {
                    _roomRepository.Edit(editedRoom);
                });
            }
        }



        [HttpDelete("{roomId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteRoom(int roomId)
        {
            return SaveData(() =>
            {
                _roomRepository.Delete(roomId);
            });
        }



        [HttpPost("filter")]
        public IEnumerable<RoomViewModel> GetAllUnreservedRooms([FromBody] RoomReservationViewModel reservationVM)
        {
            IQueryable<Room> rooms = _roomRepository.GetAll();
            var exp = new List<Room>();
            foreach (var room in rooms)
            {
                if (CheckUnreservedRoom(room.Id, DateTime.Parse(reservationVM.date), reservationVM.length))
                {
                    if (reservationVM.FilterType != "")
                    {
                        if (room.Type == reservationVM.FilterType)
                            exp.Add(room);
                    }
                    else exp.Add(room);
                }
            }
            return _mapper.Map<IEnumerable<RoomViewModel>>(exp);
        }
        #endregion



        #region Helpers

        public bool CheckUnreservedRoom(int roomId, DateTime date, int length)
        {
            IQueryable<RoomReservation> reservations = _roomReservationRepository.Get(r => r.RoomId == roomId && r.dateTime.Date == date.Date); //all reservations my car of current day 
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




        private bool ExistAnotherRoomWithName(string roomName, int roomId)
        {
            var room = _roomRepository.GetItem(roomName);

            return room != null && room.Id != roomId;
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
                _roomRepository.Save();

                return result();
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured when saving data.", ex);
                this.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return this.Json(new { Message = $"Saving data throw Exception '{ex.Message}'" });
            }
        }

        #endregion


    }
}
