using System;
using System.Collections.Generic;
using System.Linq;
using Krosbook.Models.Base;
using Krosbook.Models.Users;

namespace Krosbook.Models.Rooms.Dummies
{
    /// <summary>
    /// Room dummy repository for testing
    /// </summary>
    /// <seealso cref="Krosbook.Models.IRoomRepository" />
    /// <seealso cref="Krosbook.Models.Base.BaseDummyRepository{T}" />
    public class RoomDummyRepository : BaseDummyRepository<Room>, IRoomRepository
    {

        /// <summary>
        /// Gets room with specific name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Room with specific name; if doesn't exist then return null.</returns>
        public Room GetItem(string name)
        {
            return this.GetItem(p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Adds the specified room.
        /// </summary>
        /// <param name="room">The room.</param>
        public override void Add(Room room)
        {
            if (this.GetAll().Any(p => p.Name.Equals(room.Name, System.StringComparison.CurrentCultureIgnoreCase)))
            {
                throw new System.InvalidProgramException($"Room with name {room.Name}, already exist.");
            }

            base.Add(room);
        }

        /// <summary>
        /// Initializes the dummy data.
        /// </summary>
        /// <param name="dummyData">The dummy data.</param>
        protected override void InitDummyData(IList<Room> dummyData)
        {
            base.InitDummyData(dummyData);

            dummyData.Add(new Room() { Id = 0, Name = "Žltá školiaca", Description = "Pekná veľká" });
            dummyData.Add(new Room() { Id = 1, Name = "Modrá školiaca", Description = "Nádherná" });
            dummyData.Add(new Room() { Id = 2, Name = "Malá zasadačka", Description = "Nádherná zasadačka" });
        }

        /// <summary>
        /// Get types of rooms.
        /// </summary>
        /// <returns>Types of rooms.</returns>
        public IEnumerable<string> GetTypes()
        {
            return _data.Select(p => p.Type).Distinct();
        }



    }
}
