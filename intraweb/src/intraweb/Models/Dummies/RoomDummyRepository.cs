using System.Collections.Generic;
using System.Linq;

namespace intraweb.Models.Dummies
{
    /// <summary>
    /// Room dummy repository for testing
    /// </summary>
    public class RoomDummyRepository : IRoomRepository
    {

        private List<Room> _rooms = new List<Room>();

        public RoomDummyRepository()
        {
            CreateTestingData();
        }

        /// <summary>
        /// Adds the room.
        /// </summary>
        /// <param name="room">The new room.</param>
        /// <returns>
        /// Added room.
        /// </returns>
        public Room AddRoom(Room room)
        {
            if (_rooms.Any(p => p.Name.Equals(room.Name, System.StringComparison.CurrentCultureIgnoreCase)))
            {
                throw new System.InvalidProgramException($"Room with name {room.Name}, already exist.");
            }

            room.Id = _rooms.Max(p => p.Id) + 1;

            _rooms.Add(room);

            return room;
        }

        /// <summary>
        /// Deletes the specified room by id.
        /// </summary>
        /// <param name="roomId">The room identifier.</param>
        /// <returns></returns>
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
            _rooms.Remove(room);
        }

        /// <summary>
        /// Edits the specified room.
        /// </summary>
        /// <param name="room">The room.</param>
        public void Edit(Room room)
        {
        }

        /// <summary>
        /// Gets all rooms.
        /// </summary>
        /// <returns>
        /// Queryalble for obtain all rooms.
        /// </returns>
        public IQueryable<Room> GetAllRooms()
        {
            return _rooms.AsQueryable();
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
            return this.GetAllRooms().FirstOrDefault(p => p.Id == roomId);
        }

        /// <summary>
        /// Save changes.
        /// </summary>
        /// <returns></returns>
        public void Save()
        {
        }

        private void CreateTestingData()
        {
            _rooms.Add(new Room() { Id = 0, Name = "Žltá školiaca", Description = "Pekná veľká" });
            _rooms.Add(new Room() { Id = 1, Name = "Modrá školiaca", Description = "Nádherná" });
            _rooms.Add(new Room() { Id = 2, Name = "Malá zasadačka", Description = "Nádherná zasadačka" });
        }
    }
}
