using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System;
using IntraWeb.Filters;
using Microsoft.Extensions.Logging;
using IntraWeb.Models.Users;
using IntraWeb.ViewModels.Users;
using AutoMapper;

namespace IntraWeb.Controllers.Api.v1
{
    [Route("api/roles")]
    public class RolesController : BaseController
    {
        #region Private Fields

        private IRoleRepository _roleRepository;
        private ILogger<RolesController> _logger;
        private IMapper _mapper;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="RolesController"/> class.
        /// </summary>
        /// <param name="roleRepository">The role repository.</param>
        /// <param name="logger">Logger.</param>
        /// <param name="mapper">Mapper for mapping domain classes to model classes and reverse.</param>
        public RolesController(IRoleRepository roleRepository,
                      ILogger<RolesController> logger,
                                       IMapper mapper)
        {
            _roleRepository = roleRepository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all roles.
        /// </summary>
        /// <returns>All roles</returns>
        [HttpGet]
        public IEnumerable<RoleViewModel> Get()
        {
            return _mapper.Map<IEnumerable<RoleViewModel>>(_roleRepository.GetAll());
        }

        /// <summary>
        /// Post new role.
        /// </summary>
        /// <param name="roleVm">New role.</param>
        /// <returns>Added role.</returns>
        [HttpPost()]
        [ValidateModelState, CheckArgumentsForNull]
        //[Authorize(Roles = "Administrator")] - ToDo: Zakomentovane pokiaľ sa nespraví autorizácia
        public IActionResult Post([FromBody] RoleViewModel roleVm)
        {
            if (_roleRepository.GetItem(u => u.Name == roleVm.Name) == null)
            {
                Role role = _mapper.Map<Role>(roleVm);

                return SaveData(() =>
                {
                    _roleRepository.Add(role);
                },
                () =>
                {
                    this.Response.StatusCode = (int) HttpStatusCode.Created;
                    return this.Json(_mapper.Map<RoleViewModel>(role));
                });
            }
            else
            {
                this.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return this.Json(new { Message = $"Role with name '{roleVm.Name}' already exist." });
            }
        }

        /// <summary>
        /// Update the role.
        /// </summary>
        /// <param name="roleId">Role id for update.</param>
        /// <param name="roleVm">Role view model, with new properties.</param>
        [HttpPut("{roleId}")]
        [ValidateModelState, CheckArgumentsForNull]
        //[Authorize(Roles = "Administrator")] - ToDo: Zakomentovane pokiaľ sa nespraví autorizácia
        public IActionResult Put(int roleId, [FromBody] RoleViewModel roleVm)
        {
            if (roleVm.Id != roleId)
            {
                var message = $"Invalid argument. Id '{roleId}' and roleVm.Id '{roleVm.Id}' are not equal.";
                _logger.LogWarning(message);

                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json(new { Message = message });
            }

            Role editedRole = _roleRepository.GetItem(roleId);
            if (editedRole == null)
            {
                this.Response.StatusCode = (int) HttpStatusCode.NotFound;
                return this.Json(null);
            }

            if (this.ExistAnotherRoleWithName(roleVm.Name, roleId))
            {
                this.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return this.Json(new { Message = $"Role with name '{roleVm.Name}' already exist." });
            }
            else
            {
                editedRole = _mapper.Map(roleVm, editedRole);

                return SaveData(() =>
                {
                    _roleRepository.Edit(editedRole);
                });
            }
        }

        /// <summary>
        /// Deletes the specified role.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        [HttpDelete("{roleId}")]
        //[Authorize(Roles = "Administrator")] - ToDo: Zakomentovane pokiaľ sa nespraví autorizácia
        public IActionResult Delete(int roleId)
        {
            return SaveData(() =>
            {
                _roleRepository.Delete(roleId);
            });
        }

        private bool ExistAnotherRoleWithName(string roleName, int roleId)
        {
            Role role = _roleRepository.GetItem(u => u.Name == roleName);

            return role != null && role.Id != roleId;
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
                _roleRepository.Save();

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
