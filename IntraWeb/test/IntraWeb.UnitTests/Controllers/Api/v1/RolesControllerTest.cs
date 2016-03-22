using System;
using System.Linq;
using Xunit;
using IntraWeb.UnitTests.Service;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http.Internal;
using System.Net;
using IntraWeb.UnitTests.Filters;
using IntraWeb.Filters;
using IntraWeb.Controllers.Api.v1;
using IntraWeb.ViewModels.Users;
using IntraWeb.Models.Users;

namespace IntraWeb.UnitTests.Controllers.Api.v1
{

    public class RolesControllerTest
    {

        public RolesControllerTest()
        {
            AutoMapper.Mapper.Initialize(conf =>
            {
                conf.AddProfile<UsersMappingProfile>();
            });
        }

        #region "Get roles"

        [Fact]
        public void GetRolesReturnEmptyListWhenRolesDoesNotExist()
        {
            // Arrange
            var target = CreateRolesController(null);

            // Act
            var rolesCount = target.Get().Count();

            // Assert            
            Assert.Equal(0, rolesCount);
        }

        [Fact]
        public void GetRolesReturnAllRolesFromRepository()
        {
            // Arrange
            var target = CreateRolesController((rep) =>
            {
                rep.Add(new Role()
                {
                    Id = 0,
                    Name = "Admin"
                });
                rep.Add(new Role()
                {
                    Id = 1,
                    Name = "User"
                });
            });

            // Act 
            var roles = target.Get().ToList();

            // Assert
            Assert.Equal(2, roles.Count);

            Assert.Equal(0, roles[0].Id);
            Assert.Equal("Admin", roles[0].Name);
        }

        #endregion


        #region "Post"

        [Fact()]
        public void PostRoleAddToRepository()
        {
            // Arrange
            RoleDummyRepository repository = null;
            var target = CreateRolesController((rep) =>
            {
                rep.Add(new Role()
                {
                    Id = 0,
                    Name = "Admin"
                });
                rep.Add(new Role()
                {
                    Id = 1,
                    Name = "User"
                });

                repository = rep;
            });

            // Act
            var response = target.Post(new RoleViewModel()
            {
                Id = 2,
                Name = "ReadOnly"
            });

            // Assert
            Assert.Equal(3, repository.GetAll().Count());

            var role = repository.GetAll().Last();

            Assert.Equal(2, role.Id);
            Assert.Equal("ReadOnly", role.Name);
        }

        [Fact()]
        public void PostRoleReturnAddedRole()
        {
            // Arrange
            var target = CreateRolesController((rep) =>
            {
                rep.Add(new Role()
                {
                    Id = 0,
                    Name = "Admin"
                });
                rep.Add(new Role()
                {
                    Id = 1,
                    Name = "User"
                });
            });

            // Act
            var response = target.Post(new RoleViewModel()
            {
                Id = 2,
                Name = "ReadOnly"
            });

            // Assert
            var role = (response as JsonResult).Value as RoleViewModel;

            Assert.Equal(2, role.Id);
            Assert.Equal("ReadOnly", role.Name);
        }

        [Fact()]
        public void PostRoleCreatedStatusCode()
        {
            // Arrange
            var target = CreateRolesController((rep) =>
            {
                rep.Add(new Role()
                {
                    Id = 0,
                    Name = "Admin"
                });
                rep.Add(new Role()
                {
                    Id = 1,
                    Name = "User"
                });
            });

            // Act
            var response = target.Post(new RoleViewModel()
            {
                Id = 2,
                Name = "ReadOnly"
            });

            // Assert
            Assert.Equal((int) HttpStatusCode.Created, target.Response.StatusCode);
        }

        [Fact()]
        public void PostRoleBadRequestIfRoleWithNameExist()
        {
            // Arrange
            var target = CreateRolesController((rep) =>
            {
                rep.Add(new Role()
                {
                    Id = 0,
                    Name = "Admin"
                });
                rep.Add(new Role()
                {
                    Id = 1,
                    Name = "User"
                });
            });

            // Act
            var response = target.Post(new RoleViewModel()
            {
                Id = 2,
                Name = "User"
            });

            // Assert
            Assert.Equal((int) HttpStatusCode.BadRequest, target.Response.StatusCode);
        }

        [Fact()]
        public void PostMethodHasCheckArgumentsForNullAttribute()
        {
            // Arrange
            var target = new RolesController(null, null, null);
            Func<RoleViewModel, IActionResult> method = target.Post;

            // Act
            var hasCheckArgumentsForNullAttribute = method.HasMethodActionFilterAttribute<CheckArgumentsForNullAttribute>();

            // Assert
            Assert.True(hasCheckArgumentsForNullAttribute);
        }

        [Fact()]
        public void PostMethodHasValidateModelStateAttribute()
        {
            // Arrange
            var target = new RolesController(null, null, null);
            Func<RoleViewModel, IActionResult> method = target.Post;

            // Act
            var hasValidateModelStateAttribute = method.HasMethodActionFilterAttribute<ValidateModelStateAttribute>();

            // Assert
            Assert.True(hasValidateModelStateAttribute);
        }

        [Fact]
        public void PostInternalServerErrorStatusCode()
        {
            // Arrange
            var target = CreateRolesController((rep) =>
            {
                rep.ThrowExceptionWhenSaveData = true;

            });

            // Act
            var response = target.Post(new RoleViewModel()
            {
                Id = 2,
                Name = "ReadOnly"
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
            var target = CreateRolesController(null);

            // Act
            var response = target.Put(1, new RoleViewModel() { Id = 2, Name = "ReadOnly" });

            // Assert
            Assert.Equal((int) HttpStatusCode.BadRequest, target.Response.StatusCode);
        }

        [Fact]
        public void PutNameWhichAlreadyExist()
        {
            // Arrange
            var target = CreateRolesController(rep =>
            {
                rep.Add(new Role()
                {
                    Id = 0,
                    Name = "Admin"
                });
                rep.Add(new Role()
                {
                    Id = 1,
                    Name = "User"
                });
            });

            // Act
            var response = target.Put(1, new RoleViewModel() { Id = 1, Name = "Admin" });

            // Assert
            Assert.Equal((int) HttpStatusCode.BadRequest, target.Response.StatusCode);
        }

        [Fact]
        public void PutRoleDoesntExist()
        {
            // Arrange
            var target = CreateRolesController(rep =>
            {
                rep.Add(new Role()
                {
                    Id = 0,
                    Name = "Admin"
                });
                rep.Add(new Role()
                {
                    Id = 1,
                    Name = "User"
                });
            });

            // Act
            var response = target.Put(3, new RoleViewModel() { Id = 3, Name = "ReadOnly" });

            // Assert
            Assert.Equal((int) HttpStatusCode.NotFound, target.Response.StatusCode);
        }

        [Fact]
        public void PutRoleCorrectUpdate()
        {
            // Arrange
            RoleDummyRepository repository = null;
            var target = CreateRolesController(rep =>
            {
                rep.Add(new Role()
                {
                    Id = 0,
                    Name = "Admin"
                });
                rep.Add(new Role()
                {
                    Id = 1,
                    Name = "User"
                });

                repository = rep;
            });

            // Act
            var response = target.Put(0, new RoleViewModel() {
                Id = 0,
                Name = "NewAdmin"
            });

            var roleForTest = repository.GetAll().First();

            // Assert
            Assert.Equal("NewAdmin", roleForTest.Name);
        }

        [Fact]
        public void PutRoleCorrectUpdateOkResponseStatusCode()
        {
            // Arrange
            RoleDummyRepository repository = null;
            var target = CreateRolesController(rep =>
            {
                rep.Add(new Role()
                {
                    Id = 0,
                    Name = "Admin"
                });
                rep.Add(new Role()
                {
                    Id = 1,
                    Name = "User"
                });

                repository = rep;
            });

            // Act
            var response = target.Put(0, new RoleViewModel()
            {
                Id = 0,
                Name = "NewAdmin"
            });

            // Assert
            Assert.Equal((int) HttpStatusCode.OK, target.Response.StatusCode);
        }

        [Fact]
        public void PutInternalServerErrorStatusCode()
        {
            // Arrange
            var target = CreateRolesController((rep) =>
            {
                rep.Add(new Role()
                {
                    Id = 0,
                    Name = "Admin"
                });

                rep.ThrowExceptionWhenSaveData = true;
            });

            // Act
            var response = target.Put(0, new RoleViewModel()
            {
                Id = 0,
                Name = "NewAdmin"
            });

            // Assert
            Assert.Equal((int) HttpStatusCode.InternalServerError, target.Response.StatusCode);
        }

        [Fact()]
        public void PutMethodHasCheckArgumentsForNullAttribute()
        {
            // Arrange
            var target = new RolesController(null, null, null);
            Func<int, RoleViewModel, IActionResult> method = target.Put;

            // Act
            var hasCheckArgumentsForNullAttribute = method.HasMethodActionFilterAttribute<CheckArgumentsForNullAttribute>();

            // Assert
            Assert.True(hasCheckArgumentsForNullAttribute);
        }

        [Fact()]
        public void PutMethodHasValidateModelStateAttribute()
        {
            // Arrange
            var target = new RolesController(null, null, null);
            Func<int, RoleViewModel, IActionResult> method = target.Put;

            // Act
            var hasValidateModelStateAttribute = method.HasMethodActionFilterAttribute<ValidateModelStateAttribute>();

            // Assert
            Assert.True(hasValidateModelStateAttribute);
        }

        #endregion


        #region "Delete"

        [Fact]
        public void DeleteRoleOkResponseStatusCode()
        {
            // Arrange
            var target = CreateRolesController(rep =>
            {
                rep.Add(new Role()
                {
                    Id = 0,
                    Name = "Admin"
                });
                rep.Add(new Role()
                {
                    Id = 1,
                    Name = "User"
                });
            });

            // Act
            var response = target.Delete(1);

            // Assert
            Assert.Equal((int) HttpStatusCode.OK, target.Response.StatusCode);
        }

        [Fact]
        public void DeleteRoleCorrectRemoveRole()
        {
            // Arrange
            RoleDummyRepository repository = null;
            var target = CreateRolesController(rep =>
            {
                rep.Add(new Role()
                {
                    Id = 0,
                    Name = "Admin"
                });
                rep.Add(new Role()
                {
                    Id = 1,
                    Name = "User"
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
            var target = CreateRolesController((rep) =>
            {
                rep.Add(new Role()
                {
                    Id = 0,
                    Name = "NewAdmin"
                });

                rep.ThrowExceptionWhenSaveData = true;

            });

            // Act
            var response = target.Delete(1);

            // Assert
            Assert.Equal((int) HttpStatusCode.InternalServerError, target.Response.StatusCode);
        }

        [Fact]
        public void DeleteRoleWhichDoesntExistIsOk()
        {
            // Arrange
            var target = CreateRolesController(rep =>
            {
                rep.Add(new Role()
                {
                    Id = 0,
                    Name = "Admin"
                });
                rep.Add(new Role()
                {
                    Id = 1,
                    Name = "User"
                });
            });

            // Act
            var response = target.Delete(5);

            // Assert
            Assert.Equal((int) HttpStatusCode.OK, target.Response.StatusCode);
        }

        #endregion


        #region "Helpers"

        private RolesController CreateRolesController(Action<RoleDummyRepository> initRepository)
        {
            var logger = new StubLogger<RolesController>();
            var rolesRepository = new RoleDummyRepository();
            var userRolesRepository = new UserRoleDummyRepository();
            rolesRepository.ClearAll();

            if (initRepository != null)
            {
                initRepository(rolesRepository);
            }

            var target = new RolesController(rolesRepository, userRolesRepository, logger)
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
