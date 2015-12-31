using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;

namespace IntraWeb.Models
{
    /// <summary>
    /// Rooms repository with EF
    /// </summary>
    /// <seealso cref="IntraWeb.Models.IRoomRepository" />
    public class RoomsRepository : IRoomRepository
    {

        #region Private Fields

        private ApplicationDbContext _dbCondext;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomsRepository"/> class.
        /// </summary>
        /// <param name="dbCondext">The database condext.</param>
        public RoomsRepository(ApplicationDbContext dbCondext)
        {
            _dbCondext = dbCondext;
        }

        /// <summary>
        /// Adds the room.
        /// </summary>
        /// <param name="room">The new room.</param>
        /// <returns>
        /// Added room.
        /// </returns>
        public void AddRoom(Room room)
        {
            _dbCondext.Rooms.Add(room);
        }

        /// <summary>
        /// Deletes the specified room by id.
        /// </summary>
        /// <param name="roomId">The room identifier.</param>
        public void Delete(int roomId)
        {
            this.Delete(this.GetRoom(roomId));
        }

        /// <summary>
        /// Deletes the specified room.
        /// </summary>
        /// <param name="room">The room for deleting.</param>
        public void Delete(Room room)
        {
            _dbCondext.Rooms.Remove(room);
        }

        /// <summary>
        /// Edits the specified room.
        /// </summary>
        /// <param name="room">The room.</param>
        public void Edit(Room room)
        {
            _dbCondext.Entry(room).State = EntityState.Modified;
        }

        /// <summary>
        /// Gets all rooms.
        /// </summary>
        /// <returns>
        /// Queryalble for obtain all rooms.
        /// </returns>
        public IQueryable<Room> GetAllRooms()
        {
            return _dbCondext.Rooms;
        }

        /// <summary>
        /// Gets room with specific name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// Room with specific name; if doesn't exist then return null.
        /// </returns>
        public Room GetRoom(string name)
        {
            return _dbCondext.Rooms.FirstOrDefault(p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Gets the room.
        /// </summary>
        /// <param name="roomId">The room identifier.</param>
        /// <returns>
        /// Return room with specific id; otherwise null.
        /// </returns>
        public Room GetRoom(int roomId)
        {
            return _dbCondext.Rooms.FirstOrDefault(p => p.Id == roomId);
        }

        /// <summary>
        /// Save changes.
        /// </summary>
        public void Save()
        {
            _dbCondext.SaveChanges();
        }
    }
}
