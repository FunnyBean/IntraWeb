using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using intraweb.Models;
using intraweb.ViewModels.Administration;
using System.Net;
using System;
using intraweb.Filters;

namespace intraweb.Controllers
{
    [Route("api/rooms")]
    public class RoomsController : Controller
    {
        private IRoomRepository _roomRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomsController"/> class.
        /// </summary>
        /// <param name="roomRepository">The room repository.</param>
        public RoomsController(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        /// <summary>
        /// Gets all rooms.
        /// </summary>
        /// <returns>All rooms</returns>
        [HttpGet]
        public IEnumerable<RoomViewModel> Get()
        {
            var rooms = AutoMapper.Mapper.Map<IEnumerable<RoomViewModel>>(_roomRepository.GetAllRooms());
            return rooms;
        }

        /// <summary>
        /// Gets room by Id.
        /// </summary>
        /// <param name="roomId">Room Id.</param>
        /// <returns>Room with specific Id. Null if doesn't exist.</returns>
        [HttpGet("{roomId}", Name = "GetRoom")]
        public IActionResult Get(int roomId)
        {
            var room = _roomRepository.GetRoom(roomId);

            if (room == null)
            {
                this.Response.StatusCode = (int) HttpStatusCode.NoContent;
                return this.Json(null);
            }
            else
            {
                return this.Json(AutoMapper.Mapper.Map<RoomViewModel>(room));
            }
        }

        /// <summary>
        /// Post new room.
        /// </summary>
        /// <param name="roomVm">New room.</param>
        /// <returns>Added room.</returns>
        [HttpPost()]
        [ValidateModelState]
        [CheckArgumentsForNull]
        public IActionResult Post([FromBody] RoomViewModel roomVm)
        {

            if (_roomRepository.GetRoom(roomVm.Name) == null)
            {
                try
                {
                    var room = AutoMapper.Mapper.Map<Room>(roomVm);
                    _roomRepository.AddRoom(room);
                    _roomRepository.Save();

                    this.Response.StatusCode = (int) HttpStatusCode.Created;
                    return this.Json(AutoMapper.Mapper.Map<RoomViewModel>(room));
                }
                catch (Exception ex)
                {
                    this.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    return this.Json(new { Message = $"Saving data throw Exception '{ex.Message}'" });
                }
            }
            else
            {
                this.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return this.Json(new { Message = $"Room with name '{roomVm.Name}' already exist." });
            }


        }

        /// <summary>
        /// Update the room.
        /// </summary>
        /// <param name="roomId">Room id for update.</param>
        /// <param name="roomVm">Room view model, whit new properties.</param>
        [HttpPut("{roomId}")]
        [ValidateModelState]
        [CheckArgumentsForNull]
        public IActionResult Put(int roomId, [FromBody] RoomViewModel roomVm)
        {
            if (roomVm.Id != roomId)
            {
                this.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return this.Json(new { Message = "Invalid argument. Id and roomVm.Id are not equal." });
            }

            var room = _roomRepository.GetRoom(roomId);
            if (room == null)
            {
                this.Response.StatusCode = (int) HttpStatusCode.NoContent;
                return this.Json(null);
            }

            room = _roomRepository.GetRoom(roomVm.Name);
            if (room?.Id != roomId)
            {
                this.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return this.Json(new { Message = $"Room with name '{roomVm.Name}' already exist." });
            }
            else
            {
                room = AutoMapper.Mapper.Map<Room>(roomVm);
                try
                {
                    _roomRepository.Edit(room);
                    _roomRepository.Save();

                    return this.Json(null);
                }
                catch (Exception ex)
                {
                    this.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    return this.Json(new { Message = $"Saving data throw Exception '{ex.Message}'" });
                }

            }
        }
    }
}