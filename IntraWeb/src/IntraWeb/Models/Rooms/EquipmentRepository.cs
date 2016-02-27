using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntraWeb.Models.Base;

namespace IntraWeb.Models.Rooms
{
    /// <summary>
    /// Rooms repository with EF
    /// </summary>
    /// <seealso cref="IntraWeb.Models.IEquipmentRepository " />
    /// <seealso cref="IntraWeb.Models.Base.BaseRepository{T}" />
    public class EquipmentRepository : BaseRepository<Equipment>, IEquipmentRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public EquipmentRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {

        }
    }
}
