using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using IntraWeb.Controllers;
using IntraWeb.Models.Dummies;
using IntraWeb.UnitTests.Service;
using IntraWeb.ViewModels.Administration;

namespace IntraWeb.UnitTests.Controllers.Api
{
    public class RoomsControllerTest
    {

        public RoomsControllerTest()
        {
            AdministrationModelMapping.ConfigureRoomMapping();
        }

        #region "Get rooms"

        [Fact]
        public void GetRoomsReturnEmptyListWhenRoomsDoesnotExist()
        {
            // Arrange
            var target = CreateRoomsController(null);

            // Act
            var roomsCount = target.Get().Count();

            // Assert            
            Assert.Equal(0, roomsCount);
        }

        [Fact]
        public void GetRoomsReturnAllRoomsFromRepository()
        {
            // Arrange
            var target = CreateRoomsController((rep) =>
            {
                rep.AddRoom(new Models.Room()
                {
                    Name = "First",
                    Description = "First room"
                });
                rep.AddRoom(new Models.Room()
                {
                    Name = "Second",
                    Description = "Second room"
                });
            });

            // Act 
            var rooms = target.Get().ToList();

            // Assert
            Assert.Equal(2, rooms.Count);

            Assert.Equal(0, rooms[0].Id);
            Assert.Equal("First", rooms[0].Name);
            Assert.Equal("First room", rooms[0].Description);
        }

        #endregion


        #region "Helpers"

        private RoomsController CreateRoomsController(Action<RoomDummyRepository> initRepository)
        {
            var logger = new StubLogger<RoomsController>();
            var roomsRepository = new RoomDummyRepository();
            roomsRepository.ClearAll();

            if (initRepository != null)
            {
                initRepository(roomsRepository);
            }

            var target = new RoomsController(roomsRepository, logger);

            return target;
        }

        #endregion

    }
}
