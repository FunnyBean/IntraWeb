using System;
using System.Collections.Generic;
using System.Linq;
using IntraWeb.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace IntraWeb.Models.Rooms
{
    /// <summary>
    /// Rooms repository with EF
    /// </summary>
    /// <seealso cref="IntraWeb.Models.IRoomRepository" />
    /// <seealso cref="IntraWeb.Models.Base.BaseRepository{T}" />
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoomRepository"/> class.
        /// </summary>
        /// <param name="dbCondext">The database context.</param>
        public RoomRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        /// <summary>
        /// Gets room with specific name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// Room with specific name; if doesn't exist then return null.
        /// </returns>
        public Room GetItem(string name)
        {
            return this.GetItem(p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        ///  Gets the room by Id with equipment.
        /// </summary>
        /// <param name="roomId">The room identifier</param>
        /// <returns>
        /// Return room with equipment; otherwise null.
        /// </returns>
        public override Room GetItem(int roomId)
        {
            return _dbContext.Set<Room>().
                Include(r => r.Equipment).
                ThenInclude(e => e.Equipment).
                AsNoTracking().
                FirstOrDefault(r => r.Id == roomId);
        }

        /// <summary>
        /// Get types of rooms.
        /// </summary>
        /// <returns>Types of rooms.</returns>
        public IEnumerable<string> GetTypes()
        {
            return _dbContext.Set<Room>().Select(p => p.Type).Distinct();
        }

        /// <summary>
        /// Edits the specified item.
        /// </summary>
        /// <param name="room">The item.</param>
        public override void Edit(Room room)
        {
            var roomEquipment = room.Equipment;

            room.Equipment = null;
            base.Edit(room);

            if (roomEquipment != null)
            {
                var oldEquipment = _dbContext.Set<RoomEquipment>().Where(p => p.RoomId == room.Id);

                SetAddOrModifiedStatesForEquipment(room, roomEquipment);

                SetDeleteStateForNotUsedEquipment(roomEquipment, oldEquipment);
            }

        }

        private void SetAddOrModifiedStatesForEquipment(Room room, ICollection<RoomEquipment> roomEquipment)
        {
            foreach (var equipment in roomEquipment)
            {
                _dbContext.Entry(equipment).State =
                    HasRoomEquipment(room.Id, equipment.EquipmentId) ? EntityState.Modified : EntityState.Added;
            }
        }

        private void SetDeleteStateForNotUsedEquipment(ICollection<RoomEquipment> roomEquipment, IQueryable<RoomEquipment> oldEquipment)
        {
            foreach (var equipment in oldEquipment.Where((r) => !roomEquipment.Any(p => (p.EquipmentId == r.EquipmentId))))
            {
                _dbContext.Entry(equipment).State = EntityState.Deleted;
            }
        }

        private bool HasRoomEquipment(int roomId, int equipmentId)
        {
            return _dbContext.Set<RoomEquipment>().Any(p => p.RoomId == roomId && p.EquipmentId == equipmentId);
        }
    }
}
