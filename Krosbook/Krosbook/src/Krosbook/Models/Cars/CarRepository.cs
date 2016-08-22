using Krosbook.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Krosbook.Models.Cars
{
    public class CarRepository : BaseRepository<Car>, ICarRepository
    {
        public CarRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public Car GetSingleByName(string name)
        {
            return this.GetSingle(x => x.Name == name);
        }

        public Car GetSingleByPlate(string plate)
        {
            return this.GetSingle(x => x.Plate == plate);
        }


        public override Car GetItem(int carId)
        {
            return this.GetItem(x => x.Id == carId);
        }

        /// <summary>
        /// Gets all cars
        /// </summary>
        /// <returns>
        /// Return all cars; otherwise null.
        /// </returns>
        public override IQueryable<Car> GetAll()
        {
            return _dbContext.Set<Car>();
        }


        /// <summary>
        /// Edits the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public override void Edit(Car item)
        {
            base.Edit(item);
        }





    }
}
