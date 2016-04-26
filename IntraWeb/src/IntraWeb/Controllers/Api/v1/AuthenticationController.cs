using IntraWeb.Models.Users;
using IntraWeb.ViewModels.Users;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IntraWeb.Controllers.Api.v1
{

    [Route("api/authentication")]
    public class AuthenticationController : BaseController
    {

        #region Constants

        public const string ClaimTypeId = "Id";

        private const string AuthenticationScheme = "IntrawebAuthentication";

        #endregion


        #region Fields

        private readonly IUserRepository _userRepository;
        private ILogger<AuthenticationController> _logger;

        #endregion


        #region Constructors

        public AuthenticationController(IUserRepository userRepository, ILogger<AuthenticationController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        #endregion


        #region Api

        [HttpGet]
        [Route("test")]
        public IActionResult Test()
        {
            return new ObjectResult("Lorem ipsum");
        }

        [Authorize]
        [HttpGet]
        [Route("test2")]
        public IActionResult Test2()
        {
            return new ObjectResult("Huraaa");
        }

        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> Login(string userName, string password)
        {
            return await SignInCore(userName, password);
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel user)
        {
            return await SignInCore(user.UserName, user.Password);
        }


        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.Authentication.SignOutAsync(Startup.AuthenticationScheme);
            return Ok();
        }

        #endregion


        #region Helpers

        private async Task<IActionResult> SignInCore(string userName, string password)
        {
            User user = null;
            var resultSignIn = PasswordSignIn(userName, password, out user);
            if (resultSignIn.Succeeded)
            {
                await HttpContext.Authentication.SignInAsync(Startup.AuthenticationScheme, CreatePrincipal(user));
                return Ok();
            }
            return HttpUnauthorized();
        }


        private SignInResult PasswordSignIn(string userName, string password, out User user)
        {
            user = _userRepository.GetSingleByUsername(userName);
            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    return SignInResult.Success;
                }
            }
            return SignInResult.Failed;
        }


        private ClaimsPrincipal CreatePrincipal(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypeId, user.Id.ToString(), ClaimValueTypes.Integer32));
            return new ClaimsPrincipal(new ClaimsIdentity(claims, Startup.AuthenticationScheme));
        }


        #endregion



    }
}
