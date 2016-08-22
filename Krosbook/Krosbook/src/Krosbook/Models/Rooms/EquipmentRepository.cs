using Krosbook.Models.Base;

namespace Krosbook.Models.Rooms
{
    /// <summary>
    /// Rooms repository with EF
    /// </summary>
    /// <seealso cref="Krosbook.Models.IEquipmentRepository " />
    /// <seealso cref="Krosbook.Models.Base.BaseRepository{T}" />
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
