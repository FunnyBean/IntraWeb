using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using System.Net;
using System;
using IntraWeb.Filters;
using Microsoft.Extensions.Logging;
using IntraWeb.Models.Users;
using IntraWeb.ViewModels.Users;
using IntraWeb.Models;
using System.Linq;

namespace IntraWeb.Controllers.Api.v1
{
    [Route("api/users")]
    public class UsersController : BaseController
    {
        #region Private Fields

        private IUserRepository _userRepository;
        private IUserRoleRepository _userRoleRepository;
        private ILogger<UsersController> _logger;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="logger">Logger.</param>
        public UsersController(IUserRepository userRepository,
                           IUserRoleRepository userRoleRepository,
                      ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _logger = logger;
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>All users</returns>
        [HttpGet]
        public IEnumerable<UserViewModel> Get()
        {
            return AutoMapper.Mapper.Map<IEnumerable<UserViewModel>>(_userRepository.GetAllIncluding(x => x.UserRoles));
        }

        /// <summary>
        /// Post new user.
        /// </summary>
        /// <param name="userVm">New user.</param>
        /// <returns>Added user.</returns>
        [HttpPost()]
        [ValidateModelState, CheckArgumentsForNull]
        //[Authorize(Roles = "Administrator")] - ToDo: Zakomentovane pokiaľ sa nespraví autorizácia
        public IActionResult Post([FromBody] UserViewModel userVm)
        {
            return this.CreateNewUser(userVm);
        }

        /// <summary>
        /// Create new user.
        /// </summary>
        /// <param name="userVm">New user.</param>
        /// <returns>Info about creating of user.</returns>
        private IActionResult CreateNewUser(UserViewModel userVm)
        {
            if (_userRepository.GetItem(u => u.Email == userVm.Email) == null)
            {
                User user = AutoMapper.Mapper.Map<User>(userVm);
                user.DateCreated = DateTime.Now;
                user.Photo = DbInitializer.GetDefaultAvatar();

                return SaveData(() =>
                {
                    _userRepository.Add(user);
                },
                () =>
                {
                    this.Response.StatusCode = (int)HttpStatusCode.Created;
                    return this.Json(new
                    {
                        Data = this.Json(AutoMapper.Mapper.Map<UserViewModel>(user)),
                        StatusCode = this.Response.StatusCode
                    });
                });
            }
            else
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json(new {
                    Message = $"User with email '{userVm.Email}' already exist.",
                    StatusCode = this.Response.StatusCode
                });
            }
        }

        /// <summary>
        /// Post new users.
        /// </summary>
        /// <param name="userVms">New users.</param>
        /// <returns>Added users.</returns>
        [HttpPost("BulkInsert")]
        [ValidateModelState, CheckArgumentsForNull]
        //[Authorize(Roles = "Administrator")] - ToDo: Zakomentovane pokiaľ sa nespraví autorizácia
        public IEnumerable<IActionResult> Post([FromBody] IEnumerable<UserViewModel> userVms)
        {
            List<IActionResult> result = new List<IActionResult>();

            foreach (UserViewModel item in userVms)
            {
                JsonResult ret = (JsonResult)this.CreateNewUser(item);
                ret.StatusCode = ((dynamic)ret.Value).StatusCode;
                result.Add(ret);
            }

            return result;
        }

        /// <summary>
        /// Update the user.
        /// </summary>
        /// <param name="userId">User id for update.</param>
        /// <param name="userVm">User view model, with new properties.</param>
        /// <returns>Updated user.</returns>
        [HttpPut("{userId}")]
        [ValidateModelState, CheckArgumentsForNull]
        //[Authorize(Roles = "Administrator")] - ToDo: Zakomentovane pokiaľ sa nespraví autorizácia
        public IActionResult Put(int userId, [FromBody] UserViewModel userVm)
        {
            if (userVm.Id != userId)
            {
                var message = $"Invalid argument. Id '{userId}' and userVm.Id '{userVm.Id}' are not equal.";
                _logger.LogWarning(message);

                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json(new { Message = message });
            }

            User oldUser = _userRepository.GetItem(userId);
            if (oldUser == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.Json(null);
            }
            
            if (ExistAnotherUserWithEmail(userVm.Email, userId))
            {
                this.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return this.Json(new { Message = $"User with email '{userVm.Email}' already exist." });
            }
            else
            {
                IActionResult result;
                IEnumerable<int> newRolesIds = (userVm.UserRoles ?? new List<UserRoleViewModel>()).Select(x => x.RoleId).AsEnumerable();

                // Update user values
                userVm.UserRoles = null; // It's important for disable duplicating of roles
                User editedUser = AutoMapper.Mapper.Map(userVm, oldUser);

                result = SaveData(() =>
                {
                    _userRepository.Edit(editedUser);
                });

                // Update roles
                IEnumerable<int> oldRolesIds = _userRepository.GetItemIncluding(userId, true, x => x.UserRoles).UserRoles.Select(x => x.RoleId).AsEnumerable();

                if (!Enumerable.SequenceEqual(oldRolesIds, newRolesIds)) // Are role Ids the same?
                {
                    _userRoleRepository.Delete(x => x.UserId == oldUser.Id); // Remove old roles (new roles will be added later)
                    _userRoleRepository.Save();

                    foreach (int roleId in newRolesIds)
                    {
                        _userRoleRepository.Add(new UserRole()
                        {
                            RoleId = roleId,
                            UserId = oldUser.Id
                        });
                    }

                    _userRoleRepository.Save();
                }

                return result;
            }
        }

        /// <summary>
        /// Deletes the specified user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        [HttpDelete("{userId}")]
        //[Authorize(Roles = "Administrator")] - ToDo: Zakomentovane pokiaľ sa nespraví autorizácia
        public IActionResult Delete(int userId)
        {
            return SaveData(() =>
            {
                _userRepository.Delete(userId);
            });
        }

        private bool ExistAnotherUserWithEmail(string userEmail, int userId)
        {
            User user = _userRepository.GetItem(u => u.Email == userEmail);

            return user != null && user.Id != userId;
        }

        private IActionResult SaveData(Action beforeAction)
        {
            return SaveData(beforeAction, () => this.Json(null));
        }

        private IActionResult SaveData(Action beforeAction, Func<IActionResult> result)
        {
            try
            {
                beforeAction();
                _userRepository.Save();

                return result();
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured when saving data.", ex);
                this.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return this.Json(new { Message = $"Saving data throw Exception '{ex.Message}'" });
            }
        }
    }
}
