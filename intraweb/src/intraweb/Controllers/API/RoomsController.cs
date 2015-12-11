using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using intraweb.Models;
using intraweb.ViewModels.Administration;

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
        public IEnumerable<RoomViewModel> GetRooms()
        {
            return AutoMapper.Mapper.Map<IEnumerable<RoomViewModel>(_roomRepository.GetAllRooms());
        }
    }
}