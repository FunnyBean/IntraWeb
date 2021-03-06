﻿using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using AutoMapper;
using Xunit;

using IntraWeb.Controllers.Api.v1;
using IntraWeb.Filters;
using IntraWeb.Models.Users;
using IntraWeb.UnitTests.Filters;
using IntraWeb.UnitTests.Service;
using IntraWeb.ViewModels.Users;

namespace IntraWeb.UnitTests.Controllers.Api.v1
{

    public class UsersControllerTest
    {

        public UsersControllerTest()
        {
            AutoMapper.Mapper.Initialize(conf =>
            {
                conf.AddProfile<UsersMappingProfile>();
            });
        }

        #region "Get users"

        [Fact]
        public void GetUsersReturnEmptyListWhenUsersDoesNotExist()
        {
            // Arrange
            var target = CreateUsersController(null);

            // Act
            var usersCount = target.Get().Count();

            // Assert
            Assert.Equal(0, usersCount);
        }

        [Fact]
        public void GetUsersReturnAllUsersFromRepository()
        {
            // Arrange
            var target = CreateUsersController((rep) =>
            {
                rep.Add(new User()
                {
                    Id = 0,
                    Nickname = "Janko",
                    Surname = "Hraško",
                    Email = "janko.hrasko@gmail.com"
                });
                rep.Add(new User()
                {
                    Id = 1,
                    Nickname = "Juraj",
                    Surname = "Dlhý",
                    Email = "dlhy@gmail.com"
                });
            });

            // Act
            var users = target.Get().ToList();

            // Assert
            Assert.Equal(2, users.Count);

            Assert.Equal(0, users[0].Id);
            Assert.Equal("Janko", users[0].Nickname);
            Assert.Equal("Hraško", users[0].Surname);
            Assert.Equal("janko.hrasko@gmail.com", users[0].Email);
        }

        #endregion


        #region "Post"

        [Fact()]
        public void PostUserAddToRepository()
        {
            // Arrange
            UserDummyRepository repository = null;
            var target = CreateUsersController((rep) =>
            {
                rep.Add(new User()
                {
                    Id = 0,
                    Nickname = "Janko",
                    Surname = "Hraško",
                    Email = "janko.hrasko@gmail.com"
                });
                rep.Add(new User()
                {
                    Id = 1,
                    Nickname = "Juraj",
                    Surname = "Dlhý",
                    Email = "dlhy@gmail.com"
                });

                repository = rep;
            });

            // Act
            var response = target.Post(new UserViewModel()
            {
                Nickname = "New UserName",
                Surname = "New Surname",
                Email = "new.user@gmail.com"
            });

            // Assert
            Assert.Equal(3, repository.GetAll().Count());

            var user = repository.GetAll().Last();

            Assert.Equal(2, user.Id);
            Assert.Equal("New UserName", user.Nickname);
            Assert.Equal("New Surname", user.Surname);
            Assert.Equal("new.user@gmail.com", user.Email);
        }

        [Fact()]
        public void PostUserReturnAddedUser()
        {
            // Arrange
            var target = CreateUsersController((rep) =>
            {
                rep.Add(new User()
                {
                    Id = 0,
                    Nickname = "Janko",
                    Surname = "Hraško",
                    Email = "janko.hrasko@gmail.com"
                });
                rep.Add(new User()
                {
                    Id = 1,
                    Nickname = "Juraj",
                    Surname = "Dlhý",
                    Email = "dlhy@gmail.com"
                });
            });

            // Act
            var response = target.Post(new UserViewModel()
            {
                Nickname = "New UserName",
                Surname = "New Surname",
                Email = "new.user@gmail.com"
            });

            // Assert
            var user = (((response as JsonResult).Value as JsonResult).Value as JsonResult).Value as UserViewModel;

            Assert.Equal(2, user.Id);
            Assert.Equal("New UserName", user.Nickname);
            Assert.Equal("New Surname", user.Surname);
            Assert.Equal("new.user@gmail.com", user.Email);
        }

        [Fact()]
        public void PostUserCreatedStatusCode()
        {
            // Arrange
            var target = CreateUsersController((rep) =>
            {
                rep.Add(new User()
                {
                    Id = 0,
                    Nickname = "Janko",
                    Surname = "Hraško",
                    Email = "janko.hrasko@gmail.com"
                });
                rep.Add(new User()
                {
                    Id = 1,
                    Nickname = "Juraj",
                    Surname = "Dlhý",
                    Email = "dlhy@gmail.com"
                });
            });

            // Act
            var response = target.Post(new UserViewModel()
            {
                Nickname = "New UserName",
                Surname = "New Surname",
                Email = "new.user@gmail.com"
            });

            // Assert
            Assert.Equal((int) HttpStatusCode.Created, target.Response.StatusCode);
        }

        [Fact()]
        public void PostUserBadRequestIfUserWithEmailExist()
        {
            // Arrange
            var target = CreateUsersController((rep) =>
            {
                rep.Add(new User()
                {
                    Id = 0,
                    Nickname = "Janko",
                    Surname = "Hraško",
                    Email = "janko.hrasko@gmail.com"
                });
                rep.Add(new User()
                {
                    Id = 1,
                    Nickname = "Juraj",
                    Surname = "Dlhý",
                    Email = "dlhy@gmail.com"
                });
            });

            // Act
            var response = target.Post(new UserViewModel()
            {
                Nickname = "New UserName",
                Surname = "New Surname",
                Email = "janko.hrasko@gmail.com"
            });

            // Assert
            Assert.Equal((int) HttpStatusCode.BadRequest, target.Response.StatusCode);
        }

        [Fact()]
        public void PostMethodHasCheckArgumentsForNullAttribute()
        {
            // Arrange
            var target = new UsersController(null, null, null);
            Func<UserViewModel, IActionResult> method = target.Post;

            // Act
            var hasCheckArgumentsForNullAttribute = method.HasMethodActionFilterAttribute<CheckArgumentsForNullAttribute>();

            // Assert
            Assert.True(hasCheckArgumentsForNullAttribute);
        }

        [Fact()]
        public void PostMethodHasValidateModelStateAttribute()
        {
            // Arrange
            var target = new UsersController(null, null, null);
            Func<UserViewModel, IActionResult> method = target.Post;

            // Act
            var hasValidateModelStateAttribute = method.HasMethodActionFilterAttribute<ValidateModelStateAttribute>();

            // Assert
            Assert.True(hasValidateModelStateAttribute);
        }

        [Fact]
        public void PostInternalServerErrorStatusCode()
        {
            // Arrange
            var target = CreateUsersController((rep) =>
            {
                rep.ThrowExceptionWhenSaveData = true;

            });

            // Act
            var response = target.Post(new UserViewModel()
            {
                Nickname = "New UserName",
                Surname = "New Surname",
                Email = "janko.hrasko@gmail.com"
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
            var target = CreateUsersController(null);

            // Act
            var response = target.Put(0, new UserViewModel() { Id = 1, Email = "dlhy@gmail.com" });

            // Assert
            Assert.Equal((int) HttpStatusCode.BadRequest, target.Response.StatusCode);
        }

        [Fact]
        public void PutEmailWhichAlreadyExist()
        {
            // Arrange
            var target = CreateUsersController(rep =>
            {
                rep.Add(new User()
                {
                    Id = 0,
                    Nickname = "Janko",
                    Surname = "Hraško",
                    Email = "janko.hrasko@gmail.com"
                });
                rep.Add(new User()
                {
                    Id = 1,
                    Nickname = "Juraj",
                    Surname = "Dlhý",
                    Email = "dlhy@gmail.com"
                });
            });

            // Act
            var response = target.Put(0, new UserViewModel() { Id = 0, Email = "dlhy@gmail.com" });

            // Assert
            Assert.Equal((int) HttpStatusCode.BadRequest, target.Response.StatusCode);
        }

        [Fact]
        public void PutUserDoesntExist()
        {
            // Arrange
            var target = CreateUsersController(rep =>
            {
                rep.Add(new User()
                {
                    Id = 0,
                    Nickname = "Janko",
                    Surname = "Hraško",
                    Email = "janko.hrasko@gmail.com"
                });
                rep.Add(new User()
                {
                    Id = 1,
                    Nickname = "Juraj",
                    Surname = "Dlhý",
                    Email = "dlhy@gmail.com"
                });
            });

            // Act
            var response = target.Put(2, new UserViewModel() { Id = 2, Email = "new.user@gmail.com" });

            // Assert
            Assert.Equal((int) HttpStatusCode.NotFound, target.Response.StatusCode);
        }

        [Fact]
        public void PutUserCorrectUpdate()
        {
            // Arrange
            UserDummyRepository repository = null;
            var target = CreateUsersController(rep =>
            {
                rep.Add(new User()
                {
                    Id = 0,
                    Nickname = "Janko",
                    Surname = "Hraško",
                    Email = "janko.hrasko@gmail.com"
                });
                rep.Add(new User()
                {
                    Id = 1,
                    Nickname = "Juraj",
                    Surname = "Dlhý",
                    Email = "dlhy@gmail.com"
                });

                repository = rep;
            });

            // Act
            var response = target.Put(0, new UserViewModel() {
                Id = 0,
                Nickname = "New UserName",
                Surname = "New Surname",
                Email = "janko.hrasko@gmail.com"
            });

            var userForTest = repository.GetAll().First();

            // Assert
            Assert.Equal("New UserName", userForTest.Nickname);
            Assert.Equal("New Surname", userForTest.Surname);
            Assert.Equal("janko.hrasko@gmail.com", userForTest.Email);
        }

        [Fact]
        public void PutUserAndRemoveRoleCorrectUpdate()
        {
            // Arrange
            UserDummyRepository repository = null;
            var target = CreateUsersController(rep =>
            {
                rep.Add(new User()
                {
                    Id = 0,
                    Nickname = "Janko",
                    Surname = "Hraško",
                    Email = "janko.hrasko@gmail.com",
                    Roles = new List<UserRole>()
                    {
                        new UserRole()
                        {
                            RoleId = 0,
                            UserId = 0
                        },
                        new UserRole()
                        {
                            RoleId = 1,
                            UserId = 0
                        }
                    }
                });

                repository = rep;
            });

            // Act
            var response = target.Put(0, new UserViewModel()
            {
                Id = 0,
                Nickname = "Janko",
                Surname = "Hraško",
                Email = "janko.hrasko@gmail.com",
                Roles = new List<UserRoleViewModel>()
                {
                    new UserRoleViewModel()
                    {
                        RoleId = 0
                    }
                }
            });

            var userForTest = repository.GetAll().First();

            // Assert
            Assert.Equal(1, userForTest.Roles.Count());
            Assert.Equal(0, userForTest.Roles.First().RoleId);
        }

        [Fact]
        public void PutUserAndAddRoleCorrectUpdate()
        {
            // Arrange
            UserDummyRepository repository = null;
            var target = CreateUsersController(rep =>
            {
                rep.Add(new User()
                {
                    Id = 0,
                    Nickname = "Janko",
                    Surname = "Hraško",
                    Email = "janko.hrasko@gmail.com",
                    Roles = new List<UserRole>()
                    {
                        new UserRole()
                        {
                            RoleId = 0,
                            UserId = 0
                        }
                    }
                });

                repository = rep;
            });

            // Act
            var response = target.Put(0, new UserViewModel()
            {
                Id = 0,
                Nickname = "Janko",
                Surname = "Hraško",
                Email = "janko.hrasko@gmail.com",
                Roles = new List<UserRoleViewModel>()
                {
                    new UserRoleViewModel()
                    {
                        RoleId = 0
                    },
                    new UserRoleViewModel()
                    {
                        RoleId = 1
                    }
                }
            });

            var userForTest = repository.GetAll().First();

            // Assert
            Assert.Equal(2, userForTest.Roles.Count());
            Assert.Equal(0, userForTest.Roles.First().RoleId);
            Assert.Equal(1, userForTest.Roles.Last().RoleId);
        }

        [Fact]
        public void PutUserCorrectUpdateOkResponseStatusCode()
        {
            // Arrange
            UserDummyRepository repository = null;
            var target = CreateUsersController(rep =>
            {
                rep.Add(new User()
                {
                    Id = 0,
                    Nickname = "Janko",
                    Surname = "Hraško",
                    Email = "janko.hrasko@gmail.com"
                });
                rep.Add(new User()
                {
                    Id = 1,
                    Nickname = "Juraj",
                    Surname = "Dlhý",
                    Email = "dlhy@gmail.com"
                });

                repository = rep;
            });

            // Act
            var response = target.Put(0, new UserViewModel()
            {
                Id = 0,
                Nickname = "New UserName",
                Surname = "New Surname",
                Email = "janko.hrasko@gmail.com"
            });

            // Assert
            Assert.Equal((int) HttpStatusCode.OK, target.Response.StatusCode);
        }

        [Fact]
        public void PutInternalServerErrorStatusCode()
        {
            // Arrange
            var target = CreateUsersController((rep) =>
            {
                rep.Add(new User()
                {
                    Id = 0,
                    Nickname = "Janko",
                    Surname = "Hraško",
                    Email = "janko.hrasko@gmail.com"
                });

                rep.ThrowExceptionWhenSaveData = true;
            });

            // Act
            var response = target.Put(0, new UserViewModel()
            {
                Id = 0,
                Nickname = "New UserName",
                Surname = "New Surname",
                Email = "janko.hrasko@gmail.com"
            });

            // Assert
            Assert.Equal((int) HttpStatusCode.InternalServerError, target.Response.StatusCode);
        }

        [Fact()]
        public void PutMethodHasCheckArgumentsForNullAttribute()
        {
            // Arrange
            var target = new UsersController(null, null, null);
            Func<int, UserViewModel, IActionResult> method = target.Put;

            // Act
            var hasCheckArgumentsForNullAttribute = method.HasMethodActionFilterAttribute<CheckArgumentsForNullAttribute>();

            // Assert
            Assert.True(hasCheckArgumentsForNullAttribute);
        }

        [Fact()]
        public void PutMethodHasValidateModelStateAttribute()
        {
            // Arrange
            var target = new UsersController(null, null, null);
            Func<int, UserViewModel, IActionResult> method = target.Put;

            // Act
            var hasValidateModelStateAttribute = method.HasMethodActionFilterAttribute<ValidateModelStateAttribute>();

            // Assert
            Assert.True(hasValidateModelStateAttribute);
        }

        #endregion


        #region "Delete"

        [Fact]
        public void DeleteUserOkResponseStatusCode()
        {
            // Arrange
            var target = CreateUsersController(rep =>
            {
                rep.Add(new User()
                {
                    Id = 0,
                    Nickname = "Janko",
                    Surname = "Hraško",
                    Email = "janko.hrasko@gmail.com"
                });
                rep.Add(new User()
                {
                    Id = 1,
                    Nickname = "Juraj",
                    Surname = "Dlhý",
                    Email = "dlhy@gmail.com"
                });
            });

            // Act
            var response = target.Delete(1);

            // Assert
            Assert.Equal((int) HttpStatusCode.OK, target.Response.StatusCode);
        }

        [Fact]
        public void DeleteUserCorrectRemoveUser()
        {
            // Arrange
            UserDummyRepository repository = null;
            var target = CreateUsersController(rep =>
            {
                rep.Add(new User()
                {
                    Id = 0,
                    Nickname = "Janko",
                    Surname = "Hraško",
                    Email = "janko.hrasko@gmail.com"
                });
                rep.Add(new User()
                {
                    Id = 1,
                    Nickname = "Juraj",
                    Surname = "Dlhý",
                    Email = "dlhy@gmail.com"
                });

                repository = rep;
            });

            // Act
            var response = target.Delete(1);

            // Assert
            Assert.Equal(1, repository.GetAll().Count());
        }

        [Fact]
        public void DeleteInternalServerErrorStatusCode()
        {
            // Arrange
            var target = CreateUsersController((rep) =>
            {
                rep.Add(new User()
                {
                    Id = 0,
                    Nickname = "Janko",
                    Surname = "Hraško",
                    Email = "janko.hrasko@gmail.com"
                });

                rep.ThrowExceptionWhenSaveData = true;

            });

            // Act
            var response = target.Delete(1);

            // Assert
            Assert.Equal((int) HttpStatusCode.InternalServerError, target.Response.StatusCode);
        }

        [Fact]
        public void DeleteUserWhichDoesntExistIsOk()
        {
            // Arrange
            var target = CreateUsersController(rep =>
            {
                rep.Add(new User()
                {
                    Id = 0,
                    Nickname = "Janko",
                    Surname = "Hraško",
                    Email = "janko.hrasko@gmail.com"
                });
                rep.Add(new User()
                {
                    Id = 1,
                    Nickname = "Juraj",
                    Surname = "Dlhý",
                    Email = "dlhy@gmail.com"
                });
            });

            // Act
            var response = target.Delete(5);

            // Assert
            Assert.Equal((int) HttpStatusCode.OK, target.Response.StatusCode);
        }

        #endregion


        #region "Helpers"

        private UsersController CreateUsersController(Action<UserDummyRepository> initRepository)
        {
            var logger = new StubLogger<UsersController>();
            var usersRepository = new UserDummyRepository();
            usersRepository.ClearAll();

            if (initRepository != null)
            {
                initRepository(usersRepository);
            }

            var target = new UsersController(usersRepository, logger, CreateMapper())
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
            return TestHelper.CreateMapper(cfg =>
            {
                cfg.AddProfile<UsersMappingProfile>();
            });
        }

        #endregion

    }
}
