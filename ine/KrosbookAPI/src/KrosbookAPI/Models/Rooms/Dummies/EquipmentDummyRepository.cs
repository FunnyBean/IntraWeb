using Krosbook.Models.Base;
using Krosbook.Models.Rooms;

namespace Krosbook.Models.Rooms.Dummies
{
    /// <summary>
    /// Equipment dummy repository for testing
    /// </summary>
    /// <seealso cref="Krosbook.Models.IEquipmentRepository" />
    /// <seealso cref="Krosbook.Models.Base.BaseDummyRepository{T}" />
    public class EquipmentDummyRepository : BaseDummyRepository<Equipment>, IEquipmentRepository
    {
    }
}
