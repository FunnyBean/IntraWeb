using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using IntraWeb.Controllers;
using IntraWeb.Models.Dummies;
using IntraWeb.UnitTests.Service;
using IntraWeb.ViewModels.Administration;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http.Internal;
using System.Net;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNet.Authorization;
using IntraWeb.UnitTests.Extensions;
using IntraWeb.Filters;

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
        public void GetRoomsReturnEmptyListWhenRoomsDonotExist()
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


        #region "GetRoom"

        //Get Item

        [Fact()]
        public void GetRoomReturnCorrectRoom()
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
            var room = (target.Get(1) as JsonResult).Value as RoomViewModel;

            // Assert
            Assert.Equal(1, room.Id);
            Assert.Equal("Second", room.Name);
            Assert.Equal("Second room", room.Description);
        }

        [Fact]
        public void GetRoomNullResult()
        {
            // Arrange
            var target = CreateRoomsController(null);

            // Act
            var response = target.Get(1) as JsonResult;

            // Assert
            Assert.Null(response.Value);
        }

        [Fact]
        public void GetRoomNoContentStatusCode()
        {
            // Arrange
            var target = CreateRoomsController(null);

            // Act
            var response = target.Get(1) as JsonResult;

            // Assert
            Assert.Equal((int) HttpStatusCode.NoContent, target.Response.StatusCode);
        }

        #endregion


        #region "Post"

        [Fact()]
        public void PostRoomAddRoomToRepository()
        {
            // Arrange
            RoomDummyRepository reposiotry = null;
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

                reposiotry = rep;
            });

            // Act
            var response = target.Post(new RoomViewModel()
            {
                Name = "Third",
                Description = "Third room"
            });

            // Assert
            Assert.Equal(3, reposiotry.GetAllRooms().Count());
            var room = reposiotry.GetAllRooms().Last();
            Assert.Equal(2, room.Id);
            Assert.Equal("Third", room.Name);
            Assert.Equal("Third room", room.Description);
        }

        [Fact()]
        public void PostRoomReturnAddedRoom()
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
            var response = target.Post(new RoomViewModel()
            {
                Name = "Third",
                Description = "Third room"
            });

            // Assert
            var room = (response as JsonResult).Value as RoomViewModel;
            Assert.Equal(2, room.Id);
            Assert.Equal("Third", room.Name);
            Assert.Equal("Third room", room.Description);
        }

        [Fact()]
        public void PostRoomCreatedStatusCode()
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
            var response = target.Post(new RoomViewModel()
            {
                Name = "Third",
                Description = "Third room"
            });

            // Assert
            Assert.Equal((int) HttpStatusCode.Created, target.Response.StatusCode);
        }

        [Fact()]
        public void PostRoomBadRequestIfRoomWithNameExist()
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
            var response = target.Post(new RoomViewModel()
            {
                Name = "First",
                Description = "Third room"
            });

            // Assert
            Assert.Equal((int) HttpStatusCode.BadRequest, target.Response.StatusCode);
        }

        [Fact()]
        public void PostMethodHasCheckArgumentsForNullAttribute()
        {
            // Arrange
            var target = new RoomsController(null, null);
            Func<RoomViewModel, IActionResult> method = target.Post;

            // Act
            var hasCheckArgumentsForNullAttribute = method.HasMethodActionFilterAttribute<CheckArgumentsForNullAttribute>();

            // Assert
            Assert.True(hasCheckArgumentsForNullAttribute);
        }

        [Fact()]
        public void PostMethodHasValidateModelStateAttribute()
        {
            // Arrange
            var target = new RoomsController(null, null);
            Func<RoomViewModel, IActionResult> method = target.Post;

            // Act
            var hasValidateModelStateAttribute = method.HasMethodActionFilterAttribute<ValidateModelStateAttribute>();

            // Assert
            Assert.True(hasValidateModelStateAttribute);
        }

        [Fact]
        public void PostInternalServerErrorStatusCode()
        {
            // Arrange
            var target = CreateRoomsController((rep) =>
            {
                rep.ThrowExceptionWhenSaveData = true;

            });

            // Act
            var response = target.Post(new RoomViewModel()
            {
                Name = "First",
                Description = "Third room"
            });

            // Assert
            Assert.Equal((int) HttpStatusCode.InternalServerError, target.Response.StatusCode);
        }


        #endregion

        //ToDo: UnitTesty na autorizaciu. Az ked bude autorizacia hotova.
        //ToDo: Unit Testy pre Put 
        //ToDo: Unit testy pre Delete
        //ToDo: Unit testy pre Attributy

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

            var target = new RoomsController(roomsRepository, logger)
            {
                ActionContext = new ActionContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            return target;
        }

        #endregion

    }   
}
