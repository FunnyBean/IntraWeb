using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using IntraWeb.Controllers.Api.v1;
using IntraWeb.Models.Base;
using IntraWeb.Models.Rooms;
using IntraWeb.UnitTests.Service;
using IntraWeb.ViewModels.Rooms;
using Microsoft.AspNet.Http.Internal;
using IntraWeb.UnitTests.Filters;
using Microsoft.AspNet.Mvc;
using Xunit;
using IntraWeb.Filters;

namespace IntraWeb.UnitTests.Controllers.Api.v1
{
    public class EquipmentsControllerTest
    {
        public EquipmentsControllerTest()
        {
            AutoMapper.Mapper.Initialize(conf =>
            {
                conf.AddProfile<RoomsMappingProfile>();
            });
        }

        #region "Get equipments"

        [Fact]
        public void GetEquipmentsReturnEmptyListWhenEquipmentsDoesNotExist()
        {
            // Arrange
            var target = CreateEquipmentsController(null);

            // Act
            var equipmentsCount = target.Get().Count();

            // Assert            
            Assert.Equal(0, equipmentsCount);
        }

        [Fact]
        public void GetEquipmentsReturnAllEquipmentsFromRepository()
        {
            // Arrange
            var target = CreateEquipmentsController((rep) =>
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
            var equipments = target.Get().ToList();

            // Assert
            Assert.Equal(2, equipments.Count);

            Assert.Equal(0, equipments[0].Id);
            Assert.Equal("First equipment", equipments[0].Description);
        }

        #endregion


        #region "GetEquipment"

        [Fact()]
        public void GetEquipmentReturnCorrectEquipment()
        {
            // Arrange
            var target = CreateEquipmentsController((rep) =>
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
            var target = CreateEquipmentsController(null);

            // Act
            var response = target.Get(1) as JsonResult;

            // Assert
            Assert.Null(response.Value);
        }

        [Fact]
        public void GetEquipmentNoContentStatusCode()
        {
            // Arrange
            var target = CreateEquipmentsController(null);

            // Act
            var response = target.Get(1) as JsonResult;

            // Assert
            Assert.Equal((int) HttpStatusCode.NoContent, target.Response.StatusCode);
        }

        #endregion


        #region "Post"

        [Fact()]
        public void PostEquipmentAddToRepository()
        {
            // Arrange
            EquipmentDummyRepository reposiotry = null;
            var target = CreateEquipmentsController((rep) =>
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
            var target = CreateEquipmentsController((rep) =>
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
            var target = CreateEquipmentsController((rep) =>
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
            var target = CreateEquipmentsController((rep) =>
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
            var target = new IntraWeb.Controllers.Api.v1.EquipmentsController(null, null);
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
            var target = new EquipmentsController(null, null);
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
            var target = CreateEquipmentsController((rep) =>
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
            var target = CreateEquipmentsController(null);

            // Act
            var response = target.Put(1, new EquipmentViewModel() { Id = 5, Description = "First" });

            // Assert
            Assert.Equal((int) HttpStatusCode.BadRequest, target.Response.StatusCode);
        }

        [Fact]
        public void PutNameWhichAlreadyExist()
        {
            // Arrange
            var target = CreateEquipmentsController(rep =>
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
            var target = CreateEquipmentsController(rep =>
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
            var response = target.Put(4, new EquipmentViewModel() { Id = 4, Description = "White" });

            // Assert
            Assert.Equal((int) HttpStatusCode.NoContent, target.Response.StatusCode);
        }

        [Fact]
        public void PutEquipmentCorrectUpdate()
        {
            // Arrange
            EquipmentDummyRepository repository = null;
            var target = CreateEquipmentsController(rep =>
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
            var response = target.Put(1, new EquipmentViewModel() { Id = 1, Description = "Equipment whit white color" });
            var equipmentForTest = repository.GetItem(1);

            // Assert
            Assert.Equal("Equipment whit white color", equipmentForTest.Description);
        }

        [Fact]
        public void PutEquipmentCorrectUpdateOkResponseStatusCode()
        {
            // Arrange
            EquipmentDummyRepository repository = null;
            var target = CreateEquipmentsController(rep =>
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
                Description = "Equipment whit white color"
            });

            // Assert
            Assert.Equal((int) HttpStatusCode.OK, target.Response.StatusCode);
        }

        [Fact]
        public void PutInternalServerErrorStatusCode()
        {
            // Arrange
            var target = CreateEquipmentsController((rep) =>
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
            var target = new EquipmentsController(null, null);
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
            var target = new EquipmentsController(null, null);
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
            var target = CreateEquipmentsController(rep =>
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
            var target = CreateEquipmentsController(rep =>
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
            var target = CreateEquipmentsController((rep) =>
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
            var target = CreateEquipmentsController(rep =>
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

        private IntraWeb.Controllers.Api.v1.EquipmentsController CreateEquipmentsController(Action<EquipmentDummyRepository> initRepository)
        {
            var logger = new StubLogger<EquipmentsController>();
            var equipmentsRepository = new EquipmentDummyRepository();
            equipmentsRepository.ClearAll();

            if (initRepository != null)
            {
                initRepository(equipmentsRepository);
            }

            var target = new EquipmentsController(equipmentsRepository, logger)
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
