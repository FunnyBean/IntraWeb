using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using intraweb.Models;
using intraweb.ViewModels.Administration;
using System.Net;
using System;

namespace intraweb.Controllers
{
    [Route("api/Rooms")]
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
        /// <returns>Room with specific Id.</returns>
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
        public IActionResult Post([FromBody] RoomViewModel roomVm)
        {
            if (roomVm == null)
            {
                this.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return this.Json(new { Message = "Invalid argument. Room can not be null." });
            }
            else if (!this.ModelState.IsValid)
            {
                this.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return this.Json(new { Message = "Validation failed.", ModelState = this.ModelState });
            }
            else
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
        }
    }
}