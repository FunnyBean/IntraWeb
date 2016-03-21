using System;
using System.Collections.Generic;
using System.Net;
using AutoMapper;
using IntraWeb.Filters;
using IntraWeb.Models.Rooms;
using IntraWeb.ViewModels.Rooms;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;

namespace IntraWeb.Controllers.Api.v1
{
    [Route("api/rooms")]
    public class RoomsController : BaseController
    {
        #region Private Fields

        private IRoomRepository _roomRepository;
        private ILogger<RoomsController> _logger;
        private IMapper _mapper;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomsController"/> class.
        /// </summary>
        /// <param name="roomRepository">The room repository.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="mapper">Mapper for mapping domain classes to model classes and reverse.</param>
        public RoomsController(IRoomRepository roomRepository,
                      ILogger<RoomsController> logger,
                                       IMapper mapper)
        {
            _roomRepository = roomRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all rooms.
        /// </summary>
        /// <returns>All rooms</returns>
        [HttpGet]
        public IEnumerable<RoomViewModel> Get()
        {
            var rooms = _mapper.Map<IEnumerable<RoomViewModel>>(_roomRepository.GetAll());
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

        /// <summary>
        /// Get types of rooms.
        /// </summary>
        /// <returns>Types of rooms.</returns>
        [HttpGet("GetTypes")]
        public IEnumerable<string> GetTypes()
        {            
            return _roomRepository.GetTypes();
        }

        /// <summary>
        /// Post new room.
        /// </summary>
        /// <param name="roomVm">New room.</param>
        /// <returns>Added room.</returns>
        [HttpPost()]
        [ValidateModelState, CheckArgumentsForNull]
        //[Authorize(Roles = "Administrator")] - ToDo: Zakomentovane pokia¾ sa nespraví autorizácia
        public IActionResult Post([FromBody] RoomViewModel roomVm)
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

        /// <summary>
        /// Update the room.
        /// </summary>
        /// <param name="roomId">Room id for update.</param>
        /// <param name="roomVm">Room view model, with new properties.</param>
        [HttpPut("{roomId}")]
        [ValidateModelState, CheckArgumentsForNull]
        //[Authorize(Roles = "Administrator")] - ToDo: Zakomentovane pokiaľ sa nespraví autorizácia
        public IActionResult Put(int roomId, [FromBody] RoomViewModel roomVm)
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

        /// <summary>
        /// Deletes the specified room.
        /// </summary>
        /// <param name="roomId">The room identifier.</param>
        [HttpDelete("{roomId}")]
        //[Authorize(Roles = "Administrator")] - ToDo: Zakomentovane pokia¾ sa nespraví autorizácia
        public IActionResult Delete(int roomId)
        {
            return SaveData(() =>
            {
                _roomRepository.Delete(roomId);
            });
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
    }
}
