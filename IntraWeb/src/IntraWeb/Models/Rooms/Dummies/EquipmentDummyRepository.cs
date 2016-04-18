using IntraWeb.Models.Base;
using IntraWeb.Models.Rooms;

namespace IntraWeb.Models.Rooms.Dummies
{
    /// <summary>
    /// Equipment dummy repository for testing
    /// </summary>
    /// <seealso cref="IntraWeb.Models.IEquipmentRepository" />
    /// <seealso cref="IntraWeb.Models.Base.BaseDummyRepository{T}" />
    public class EquipmentDummyRepository : BaseDummyRepository<Equipment>, IEquipmentRepository
    {
    }
}
