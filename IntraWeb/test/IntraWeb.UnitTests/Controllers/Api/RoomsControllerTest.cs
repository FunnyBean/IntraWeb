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
using IntraWeb.UnitTests.Filters;
using IntraWeb.Filters;
using IntraWeb.Controllers.Api;

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
        public void GetRoomsReturnEmptyListWhenRoomsDoesNotExist()
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


        #region "Authoritzation"

        //ToDo: Testy na autorizaciu az ked bude autorizacia hotova.

        #endregion


        #region "Put"

        [Fact]
        public void PutIncorrectId()
        {
            // Arrange
            var target = CreateRoomsController(null);

            // Act
            var response = target.Put(1, new RoomViewModel() { Id = 5, Name = "First" });

            // Assert
            Assert.Equal((int) HttpStatusCode.BadRequest, target.Response.StatusCode);
        }

        [Fact]
        public void PutNameWhichAlreadyExist()
        {
            // Arrange
            var target = CreateRoomsController(rep =>
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
            var response = target.Put(1, new RoomViewModel() { Id = 1, Name = "First" });

            // Assert
            Assert.Equal((int) HttpStatusCode.BadRequest, target.Response.StatusCode);
        }

        [Fact]
        public void PutRoomDoesntExist()
        {
            // Arrange
            var target = CreateRoomsController(rep =>
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
            var response = target.Put(4, new RoomViewModel() { Id = 4, Name = "White" });

            // Assert
            Assert.Equal((int) HttpStatusCode.NoContent, target.Response.StatusCode);
        }

        [Fact]
        public void PutRoomCorrectUpdate()
        {
            // Arrange
            RoomDummyRepository repository = null;
            var target = CreateRoomsController(rep =>
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
                repository = rep;
            });

            // Act
            var response = target.Put(1, new RoomViewModel() { Id = 1, Name = "White", Description = "Room whit white color" });
            var roomForTest = repository.GetRoom(1);

            // Assert
            Assert.Equal("White", roomForTest.Name);
            Assert.Equal("Room whit white color", roomForTest.Description);
        }

        [Fact]
        public void PutRoomCorrectUpdateOkResponseStatusCode()
        {
            // Arrange
            RoomDummyRepository repository = null;
            var target = CreateRoomsController(rep =>
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
                repository = rep;
            });

            // Act
            var response = target.Put(1, new RoomViewModel()
            {
                Id = 1,
                Name = "White",
                Description = "Room whit white color"
            });

            // Assert
            Assert.Equal((int) HttpStatusCode.OK, target.Response.StatusCode);
        }

        [Fact]
        public void PutInternalServerErrorStatusCode()
        {
            // Arrange
            var target = CreateRoomsController((rep) =>
            {
                rep.AddRoom(new Models.Room()
                {
                    Name = "First",
                    Description = "First room"
                });
                rep.ThrowExceptionWhenSaveData = true;

            });

            // Act
            var response = target.Put(0, new RoomViewModel()
            {
                Id = 0,
                Name = "Second",
                Description = "Second room"
            });

            // Assert
            Assert.Equal((int) HttpStatusCode.InternalServerError, target.Response.StatusCode);
        }

        [Fact()]
        public void PutMethodHasCheckArgumentsForNullAttribute()
        {
            // Arrange
            var target = new RoomsController(null, null);
            Func<int, RoomViewModel, IActionResult> method = target.Put;

            // Act
            var hasCheckArgumentsForNullAttribute = method.HasMethodActionFilterAttribute<CheckArgumentsForNullAttribute>();

            // Assert
            Assert.True(hasCheckArgumentsForNullAttribute);
        }

        [Fact()]
        public void PutMethodHasValidateModelStateAttribute()
        {
            // Arrange
            var target = new RoomsController(null, null);
            Func<int, RoomViewModel, IActionResult> method = target.Put;

            // Act
            var hasValidateModelStateAttribute = method.HasMethodActionFilterAttribute<ValidateModelStateAttribute>();

            // Assert
            Assert.True(hasValidateModelStateAttribute);
        }

        #endregion


        #region "Delete"

        [Fact]
        public void DeleteRoomOkResponseStatusCode()
        {
            // Arrange
            var target = CreateRoomsController(rep =>
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
            var response = target.Delete(0);

            // Assert
            Assert.Equal((int) HttpStatusCode.OK, target.Response.StatusCode);
        }

        [Fact]
        public void DeleteRoomCorrectRemoveRoom()
        {
            // Arrange
            RoomDummyRepository repository = null;
            var target = CreateRoomsController(rep =>
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
                repository = rep;
            });

            // Act
            var response = target.Delete(0);

            // Assert
            Assert.Equal(1, repository.GetAllRooms().Count());
        }

        [Fact]
        public void DeleteInternalServerErrorStatusCode()
        {
            // Arrange
            var target = CreateRoomsController((rep) =>
            {
                rep.AddRoom(new Models.Room()
                {
                    Name = "First",
                    Description = "First room"
                });
                rep.ThrowExceptionWhenSaveData = true;

            });

            // Act
            var response = target.Delete(0);

            // Assert
            Assert.Equal((int) HttpStatusCode.InternalServerError, target.Response.StatusCode);
        }

        [Fact]
        public void DeleteRoomWhichDoesntExistIsOk()
        {
            // Arrange
            var target = CreateRoomsController(rep =>
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
            var response = target.Delete(6);

            // Assert
            Assert.Equal((int) HttpStatusCode.OK, target.Response.StatusCode);
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
