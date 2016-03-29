using System.Collections.Generic;
using IntraWeb.Models.Base;

namespace IntraWeb.Models.Rooms
{
    /// <summary>
    /// Interface, which describe repository for CRUD operations on the Room model.
    /// </summary>
    public interface IRoomRepository : IRepository<Room>
    {
        /// <summary>
        /// Gets room with specific name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Room with specific name; if doesn't exist then return null.</returns>
        Room GetItem(string name);

        /// <summary>
        /// Get types of rooms.
        /// </summary>
        /// <returns>Types of rooms.</returns>
        IEnumerable<string> GetTypes();
    }
}
