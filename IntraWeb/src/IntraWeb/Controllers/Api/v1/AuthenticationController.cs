using IntraWeb.Models.Authorization;
using IntraWeb.Models.Users;
using IntraWeb.Services.Authorization;
using IntraWeb.ViewModels.Users;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IntraWeb.Controllers.Api.v1
{
    [Route("api/[controller]")]
    public class AuthenticationController : BaseController
    {
        private readonly IMembershipService _membershipService;
        private readonly IUserRepository _userRepository;
        private ILogger<AuthenticationController> _logger;

        public AuthenticationController(IMembershipService membershipService,
            IUserRepository userRepository,
                      ILogger<AuthenticationController> logger)
        {
            _membershipService = membershipService;
            _userRepository = userRepository;
            _logger = logger;
        }


        [HttpPost("authenticate")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel user)
        {
            IActionResult _result = new ObjectResult(false);
            GenericResult _authenticationResult = null;

            try
            {
                MembershipContext _userContext = _membershipService.ValidateUser(user.Username, user.Password);

                if (_userContext.User != null)
                {
                    IEnumerable<Role> _roles = _userRepository.GetUserRoles(user.Username);
                    List<Claim> _claims = new List<Claim>();
                    foreach (Role role in _roles)
                    {
                        Claim _claim = new Claim(ClaimTypes.Role, "Admin", ClaimValueTypes.String, user.Username);
                        _claims.Add(_claim);
                    }
                    await HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(new ClaimsIdentity(_claims, CookieAuthenticationDefaults.AuthenticationScheme)),
                        new Microsoft.AspNet.Http.Authentication.AuthenticationProperties { IsPersistent = user.RememberMe });


                    _authenticationResult = new GenericResult()
                    {
                        Succeeded = true,
                        Message = "Authentication succeeded"
                    };
                }
                else
                {
                    _authenticationResult = new GenericResult()
                    {
                        Succeeded = false,
                        Message = "Authentication failed"
                    };
                }
            }
            catch (Exception ex)
            {
                _authenticationResult = new GenericResult()
                {
                    Succeeded = false,
                    Message = ex.Message
                };

                _logger.LogError("Exception occured when Login.", ex);
                this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return this.Json(new { Message = $"Login throw Exception '{ex.Message}'" });
            }

            _result = new ObjectResult(_authenticationResult);
            return _result;
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.Authentication.SignOutAsync("Cookies");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occured when Logout.", ex);
                return HttpBadRequest();
            }

        }


    }
}
