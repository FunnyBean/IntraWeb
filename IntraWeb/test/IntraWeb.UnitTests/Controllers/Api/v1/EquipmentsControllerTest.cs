using System;
using System.Linq;
using System.Net;
using AutoMapper;
using IntraWeb.Controllers.Api.v1;
using IntraWeb.Filters;
using IntraWeb.Models.Rooms;
using IntraWeb.Models.Rooms.Dummies;
using IntraWeb.UnitTests.Filters;
using IntraWeb.UnitTests.Service;
using IntraWeb.ViewModels.Rooms;
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Mvc;
using Xunit;

namespace IntraWeb.UnitTests.Controllers.Api.v1
{
    public class EquipmentControllerTest
    {
        #region "Get equipment"

        [Fact]
        public void GetEquipmentReturnEmptyListWhenEquipmentDoesNotExist()
        {
            // Arrange
            var target = CreateEquipmentController(null);

            // Act
            var equipmentCount = target.Get().Count();

            // Assert
            Assert.Equal(0, equipmentCount);
        }

        [Fact]
        public void GetEquipmentReturnAllEquipmentFromRepository()
        {
            // Arrange
            var target = CreateEquipmentController((rep) =>
            {
                rep.Add(new Equipment()
                {
                    Description = "First equipment"
                });
                rep.Add(new Equipment()
                {
                    Description = "Second equipment"
                });
            });

            // Act
            var equipment = target.Get().ToList();

            // Assert
            Assert.Equal(2, equipment.Count);

            Assert.Equal(0, equipment[0].Id);
            Assert.Equal("First equipment", equipment[0].Description);
        }

        #endregion


        #region "GetEquipment"

        [Fact()]
        public void GetEquipmentReturnCorrectEquipment()
        {
            // Arrange
            var target = CreateEquipmentController((rep) =>
            {
                rep.Add(new Equipment()
                {
                    Description = "First equipment"
                });
                rep.Add(new Equipment()
                {
                    Description = "Second equipment"
                });
            });

            // Act
            var equipment = (target.Get(1) as JsonResult).Value as EquipmentViewModel;

            // Assert
            Assert.Equal(1, equipment.Id);
            Assert.Equal("Second equipment", equipment.Description);
        }

        [Fact]
        public void GetEquipmentNullResult()
        {
            // Arrange
            var target = CreateEquipmentController(null);

            // Act
            var response = target.Get(1) as JsonResult;

            // Assert
            Assert.Null(response.Value);
        }

        [Fact]
        public void GetEquipmentNotFoundStatusCode()
        {
            // Arrange
            var target = CreateEquipmentController(null);

            // Act
            var response = target.Get(1) as JsonResult;

            // Assert
            Assert.Equal((int) HttpStatusCode.NotFound, target.Response.StatusCode);
        }

        #endregion


        #region "Post"

        [Fact()]
        public void PostEquipmentAddToRepository()
        {
            // Arrange
            EquipmentDummyRepository reposiotry = null;
            var target = CreateEquipmentController((rep) =>
            {
                rep.Add(new Equipment()
                {
                    Description = "First equipment"
                });
                rep.Add(new Equipment()
                {
                    Description = "Second equipment"
                });

                reposiotry = rep;
            });

            // Act
            var response = target.Post(new EquipmentViewModel()
            {
                Description = "Third equipment"
            });

            // Assert
            Assert.Equal(3, reposiotry.GetAll().Count());
            var equipment = reposiotry.GetAll().Last();
            Assert.Equal(2, equipment.Id);
            Assert.Equal("Third equipment", equipment.Description);
        }

        [Fact()]
        public void PostEquipmentReturnAddedEquipment()
        {
            // Arrange
            var target = CreateEquipmentController((rep) =>
            {
                rep.Add(new Equipment()
                {
                    Description = "First equipment"
                });
                rep.Add(new Equipment()
                {
                    Description = "Second equipment"
                });

            });

            // Act
            var response = target.Post(new EquipmentViewModel()
            {
                Description = "Third equipment"
            });

            // Assert
            var equipment = (response as JsonResult).Value as EquipmentViewModel;
            Assert.Equal(2, equipment.Id);
            Assert.Equal("Third equipment", equipment.Description);
        }

        [Fact()]
        public void PostEquipmentCreatedStatusCode()
        {
            // Arrange
            var target = CreateEquipmentController((rep) =>
            {
                rep.Add(new Equipment()
                {
                    Description = "First equipment"
                });
                rep.Add(new Equipment()
                {
                    Description = "Second equipment"
                });

            });

            // Act
            var response = target.Post(new EquipmentViewModel()
            {
                Description = "Third equipment"
            });

            // Assert
            Assert.Equal((int) HttpStatusCode.Created, target.Response.StatusCode);
        }

        [Fact()]
        public void PostEquipmentBadRequestIfEquipmentWithNameExist()
        {
            // Arrange
            var target = CreateEquipmentController((rep) =>
            {
                rep.Add(new Equipment()
                {
                    Description = "First equipment"
                });
                rep.Add(new Equipment()
                {
                    Description = "Second equipment"
                });

            });

            // Act
            var response = target.Post(new EquipmentViewModel()
            {
                Description = "Second equipment"
            });

            // Assert
            Assert.Equal((int) HttpStatusCode.BadRequest, target.Response.StatusCode);
        }

        [Fact()]
        public void PostMethodHasCheckArgumentsForNullAttribute()
        {
            // Arrange
            var target = new EquipmentController(null, null, null);
            Func<EquipmentViewModel, IActionResult> method = target.Post;

            // Act
            var hasCheckArgumentsForNullAttribute = method.HasMethodActionFilterAttribute<CheckArgumentsForNullAttribute>();

            // Assert
            Assert.True(hasCheckArgumentsForNullAttribute);
        }

        [Fact()]
        public void PostMethodHasValidateModelStateAttribute()
        {
            // Arrange
            var target = new EquipmentController(null, null, null);
            Func<EquipmentViewModel, IActionResult> method = target.Post;

            // Act
            var hasValidateModelStateAttribute = method.HasMethodActionFilterAttribute<ValidateModelStateAttribute>();

            // Assert
            Assert.True(hasValidateModelStateAttribute);
        }

        [Fact]
        public void PostInternalServerErrorStatusCode()
        {
            // Arrange
            var target = CreateEquipmentController((rep) =>
            {
                rep.ThrowExceptionWhenSaveData = true;

            });

            // Act
            var response = target.Post(new EquipmentViewModel()
            {
                Description = "Third equipment"
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
            var target = CreateEquipmentController(null);

            // Act
            var response = target.Put(1, new EquipmentViewModel() { Id = 5, Description = "First" });

            // Assert
            Assert.Equal((int) HttpStatusCode.BadRequest, target.Response.StatusCode);
        }

        [Fact]
        public void PutNameWhichAlreadyExist()
        {
            // Arrange
            var target = CreateEquipmentController(rep =>
            {
                rep.Add(new Equipment()
                {
                    Description = "First equipment"
                });
                rep.Add(new Equipment()
                {
                    Description = "Second equipment"
                });
            });

            // Act
            var response = target.Put(1, new EquipmentViewModel() { Id = 1, Description = "First equipment" });

            // Assert
            Assert.Equal((int) HttpStatusCode.BadRequest, target.Response.StatusCode);
        }

        [Fact]
        public void PutEquipmentDoesntExist()
        {
            // Arrange
            var target = CreateEquipmentController(rep =>
            {
                rep.Add(new Equipment()
                {
                    Description = "First equipment"
                });
                rep.Add(new Equipment()
                {
                    Description = "Second equipment"
                });
            });

            // Act
            var response = target.Put(4, new EquipmentViewModel() { Id = 4, Description = "TV" });

            // Assert
            Assert.Equal((int) HttpStatusCode.NotFound, target.Response.StatusCode);
        }

        [Fact]
        public void PutEquipmentCorrectUpdate()
        {
            // Arrange
            EquipmentDummyRepository repository = null;
            var target = CreateEquipmentController(rep =>
            {
                rep.Add(new Equipment()
                {
                    Description = "First equipment"
                });
                rep.Add(new Equipment()
                {
                    Description = "Second equipment"
                });
                repository = rep;
            });

            // Act
            var response = target.Put(1, new EquipmentViewModel() { Id = 1, Description = "TV" });
            var equipmentForTest = repository.GetItem(1);

            // Assert
            Assert.Equal("TV", equipmentForTest.Description);
        }

        [Fact]
        public void PutEquipmentCorrectUpdateOkResponseStatusCode()
        {
            // Arrange
            EquipmentDummyRepository repository = null;
            var target = CreateEquipmentController(rep =>
            {
                rep.Add(new Equipment()
                {
                    Description = "First equipment"
                });
                rep.Add(new Equipment()
                {
                    Description = "Second equipment"
                });
                repository = rep;
            });

            // Act
            var response = target.Put(1, new EquipmentViewModel()
            {
                Id = 1,
                Description = "TV"
            });

            // Assert
            Assert.Equal((int) HttpStatusCode.OK, target.Response.StatusCode);
        }

        [Fact]
        public void PutInternalServerErrorStatusCode()
        {
            // Arrange
            var target = CreateEquipmentController((rep) =>
            {
                rep.Add(new Equipment()
                {
                    Description = "First equipment"
                });
                rep.ThrowExceptionWhenSaveData = true;

            });

            // Act
            var response = target.Put(0, new EquipmentViewModel()
            {
                Id = 0,
                Description = "Second equipment"
            });

            // Assert
            Assert.Equal((int) HttpStatusCode.InternalServerError, target.Response.StatusCode);
        }

        [Fact()]
        public void PutMethodHasCheckArgumentsForNullAttribute()
        {
            // Arrange
            var target = new EquipmentController(null, null, null);
            Func<int, EquipmentViewModel, IActionResult> method = target.Put;

            // Act
            var hasCheckArgumentsForNullAttribute = method.HasMethodActionFilterAttribute<CheckArgumentsForNullAttribute>();

            // Assert
            Assert.True(hasCheckArgumentsForNullAttribute);
        }

        [Fact()]
        public void PutMethodHasValidateModelStateAttribute()
        {
            // Arrange
            var target = new EquipmentController(null, null, null);
            Func<int, EquipmentViewModel, IActionResult> method = target.Put;

            // Act
            var hasValidateModelStateAttribute = method.HasMethodActionFilterAttribute<ValidateModelStateAttribute>();

            // Assert
            Assert.True(hasValidateModelStateAttribute);
        }

        #endregion


        #region "Delete"

        [Fact]
        public void DeleteEquipmentOkResponseStatusCode()
        {
            // Arrange
            var target = CreateEquipmentController(rep =>
            {
                rep.Add(new Equipment()
                {
                    Description = "First equipment"
                });
                rep.Add(new Equipment()
                {
                    Description = "Second equipment"
                });
            });

            // Act
            var response = target.Delete(0);

            // Assert
            Assert.Equal((int) HttpStatusCode.OK, target.Response.StatusCode);
        }

        [Fact]
        public void DeleteEquipmentCorrectRemoveEquipment()
        {
            // Arrange
            EquipmentDummyRepository repository = null;
            var target = CreateEquipmentController(rep =>
            {
                rep.Add(new Equipment()
                {
                    Description = "First equipment"
                });
                rep.Add(new Equipment()
                {
                    Description = "Second equipment"
                });
                repository = rep;
            });

            // Act
            var response = target.Delete(0);

            // Assert
            Assert.Equal(1, repository.GetAll().Count());
        }

        [Fact]
        public void DeleteInternalServerErrorStatusCode()
        {
            // Arrange
            var target = CreateEquipmentController((rep) =>
            {
                rep.Add(new Equipment()
                {
                    Description = "First equipment"
                });
                rep.ThrowExceptionWhenSaveData = true;

            });

            // Act
            var response = target.Delete(0);

            // Assert
            Assert.Equal((int) HttpStatusCode.InternalServerError, target.Response.StatusCode);
        }

        [Fact]
        public void DeleteEquipmentWhichDoesntExistIsOk()
        {
            // Arrange
            var target = CreateEquipmentController(rep =>
            {
                rep.Add(new Equipment()
                {
                    Description = "First equipment"
                });
                rep.Add(new Equipment()
                {
                    Description = "Second equipment"
                });
            });

            // Act
            var response = target.Delete(6);

            // Assert
            Assert.Equal((int) HttpStatusCode.OK, target.Response.StatusCode);
        }

        #endregion


        #region "Helpers"

        private IntraWeb.Controllers.Api.v1.EquipmentController CreateEquipmentController(Action<EquipmentDummyRepository> initRepository)
        {
            var logger = new StubLogger<EquipmentController>();
            var equipmentRepository = new EquipmentDummyRepository();
            equipmentRepository.ClearAll();

            if (initRepository != null)
            {
                initRepository(equipmentRepository);
            }

            var target = new EquipmentController(equipmentRepository, logger, CreateMapper())
            {
                ActionContext = new ActionContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            return target;
        }

        private IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RoomsMappingProfile>();
            });

            return config.CreateMapper();
        }

        #endregion
    }
}
