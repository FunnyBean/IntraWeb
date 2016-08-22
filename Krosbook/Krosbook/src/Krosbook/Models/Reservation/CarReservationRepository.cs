using Krosbook.Models.Base;
using Krosbook.Models.Reservation;
using System;
using System.Linq;

namespace Krosbook.Models.Reservation
{
    /// <summary>
    /// Rooms repository with EF
    /// </summary>
    /// <seealso cref="ICarReservationRepository " />
    /// <seealso cref="Krosbook.Models.Base.BaseRepository{T}" />
    public class CarReservationRepository : BaseRepository<CarReservation>, ICarReservationRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CarReservationRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public CarReservationRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        /// <summary>
        /// Gets all cars
        /// </summary>
        /// <returns>
        /// Return all cars; otherwise null.
        /// </returns>
        public override IQueryable<CarReservation> GetAll()
        {
            return _dbContext.Set<CarReservation>();
        }


        /// <summary>
        /// Edits the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public override void Edit(CarReservation item)
        {
            base.Edit(item);
        }


        public IQueryable<CarReservation> GetReservationsByCar(int carId)
        {
            return this.Get(r => r.CarId == carId);
        }

        public IQueryable<CarReservation> GetReservationsByCarInTimeInterval(int carId, DateTime from, DateTime to)
        {
            return this.Get(r => r.CarId == carId && r.dateTime >= from && r.dateTime <= to);
        }


    }

}
