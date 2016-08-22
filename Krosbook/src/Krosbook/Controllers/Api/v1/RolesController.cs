using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System;
using Krosbook.Filters;
using Microsoft.Extensions.Logging;
using Krosbook.Models.Users;
using Krosbook.ViewModels.Users;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace Krosbook.Controllers.Api.v1
{
    [Route("api/roles")]
    [EnableCors("AllowAll")]
    [Authorize(Roles = "Admin")]
    public class RolesController : BaseController
    {
        #region Private Fields

        private IRoleRepository _roleRepository;
        private ILogger<RolesController> _logger;
        private IMapper _mapper;

        #endregion

        #region Constructor
        public RolesController(IRoleRepository roleRepository,
                      ILogger<RolesController> logger,
                                       IMapper mapper)
        {
            _roleRepository = roleRepository;
            _logger = logger;
            _mapper = mapper;
        }
        #endregion


        #region API

        [HttpGet]
        public IEnumerable<RoleViewModel> GetAllRoles()
        {
            return _mapper.Map<IEnumerable<RoleViewModel>>(_roleRepository.GetAll());
        }


        [HttpPost()]
        [ValidateModelState, CheckArgumentsForNull] 
        public IActionResult CreateNewRole([FromBody] RoleViewModel roleVm)
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




        [HttpGet("{roleId}")]
        public IActionResult GetRoleById(int roleId)
        {
            var role = _roleRepository.GetItem(roleId);

            if (role == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.Json(null);
            }
            else
            {
                return this.Json(_mapper.Map<RoleViewModel>(role));
            }
        }



        [HttpPut("{roleId}")]
        [ValidateModelState, CheckArgumentsForNull]
        public IActionResult UpdateRole(int roleId, [FromBody] RoleViewModel roleVm)
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


        [HttpDelete("{roleId}")]
        public IActionResult DeleteRole(int roleId)
        {
            return SaveData(() =>
            {
                _roleRepository.Delete(roleId);
            });
        }


        #endregion

        #region Helpers

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

        #endregion
    }
}
