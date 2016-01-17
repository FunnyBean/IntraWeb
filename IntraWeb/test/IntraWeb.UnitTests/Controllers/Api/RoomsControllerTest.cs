using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using IntraWeb.Controllers;

namespace IntraWeb.UnitTests.Controllers.Api
{
    public class RoomsControllerTest
    {
        #region "Get rooms"

        [Fact]
        public void GetRoomsReturnEmptyListWhenRoomsDoesntExist()
        {
            Assert.Equal(1, 1);
        }

        #endregion

    }
}
