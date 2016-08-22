using Krosbook.Models.Base;
using Krosbook.Models.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Krosbook.Models.Reservation
{
    /// <summary>
    /// Interface, which describe repository for CRUD operations on the Equipment model.
    /// </summary>
    public interface IRoomReservationRepository : IRepository<RoomReservation>
    {
        IQueryable<RoomReservation> GetReservationsByRoom(int roomId);

        IQueryable<RoomReservation> GetReservationsByRoomInTimeInterval(int roomId, DateTime from, DateTime to);
    }
}
