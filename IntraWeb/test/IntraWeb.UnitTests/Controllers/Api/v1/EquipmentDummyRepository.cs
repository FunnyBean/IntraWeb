using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntraWeb.Models.Base;
using IntraWeb.Models.Rooms;

namespace IntraWeb.UnitTests.Controllers.Api.v1
{
    public class EquipmentDummyRepository : BaseDummyRepository<Equipment>, IEquipmentRepository
    {
    }
}
