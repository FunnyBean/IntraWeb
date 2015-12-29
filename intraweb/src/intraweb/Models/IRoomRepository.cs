using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace intraweb.Models
{
    /// <summary>
    /// Interface, which describe repository for CRUD operations on the Room model.
    /// </summary>
    public interface IRoomRepository
    {
        /// <summary>
        /// Gets all rooms.
        /// </summary>
        /// <returns>Queryalble for obtain all rooms.</returns>
        IQueryable<Room> GetAllRooms();

        /// <summary>
        /// Gets the room.
        /// </summary>
        /// <param name="roomId">The room identifier.</param>
        /// <returns>
        /// Return room with specific id; otherwise null.
        /// </returns>
        Room GetRoom(int roomId);

        /// <summary>
        /// Gets room with specific name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Room with specific name; if doesn't exist then return null.</returns>
        Room GetRoom(string name);

        /// <summary>
        /// Adds the room.
        /// </summary>
        /// <param name="room">The new room.</param>
        /// <returns>Added room.</returns>
        void AddRoom(Room room);

        /// <summary>
        /// Edits the specified room.
        /// </summary>
        /// <param name="room">The room.</param>
        void  Edit(Room room);

        /// <summary>
        /// Deletes the specified room.
        /// </summary>
        /// <param name="room">The room for deleting.</param>
        void Delete(Room room);

        /// <summary>
        /// Deletes the specified room by id.
        /// </summary>
        /// <param name="roomId">The room identifier.</param>
        void Delete(int roomId);

        /// <summary>
        /// Save changes.
        /// </summary>
        void Save();
    }
}
