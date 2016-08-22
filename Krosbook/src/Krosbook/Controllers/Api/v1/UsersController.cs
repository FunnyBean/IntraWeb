using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System;
using Krosbook.Filters;
using Microsoft.Extensions.Logging;
using Krosbook.Models.Users;
using Krosbook.ViewModels.Users;
using Krosbook.Models;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.IdentityModel.Claims;
using Krosbook.Services.Email;
using Krosbook.Services.Template;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Krosbook.Controllers.Api.v1
{
    [Route("api/users")]
    [EnableCors("AllowAll")]
    [Authorize]
    public class UsersController : BaseController
    {
        #region Private Fields

        private IUserRepository _userRepository;
        private ILogger<UsersController> _logger;
        private IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private SendEmailService _emailService;
        private IRememberMeRepository _rememberMeRepository;

        #endregion

        #region Constructor
        public UsersController(
            IUserRepository userRepository,
            ILogger<UsersController> logger,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService,
            IEmailCreator creator,
            IEmailSender sender, IRememberMeRepository rememberMeRepository)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = new SendEmailService(emailService, creator, sender);
            _rememberMeRepository = rememberMeRepository;
        }
        #endregion


        #region API
        [HttpGet]
        public IEnumerable<UserViewModel> Get()
        {
            return _mapper.Map<IEnumerable<UserViewModel>>(_userRepository.GetAll());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost()]
        [ValidateModelState, CheckArgumentsForNull]
        public IActionResult PostNewUser([FromBody] UserViewModel userVm)
        {
            return this.CreateNewUser(userVm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("BulkInsert")]
        [ValidateModelState, CheckArgumentsForNull]
        public IEnumerable<IActionResult> PostNewUserByBulkInsert([FromBody] IEnumerable<UserViewModel> userVms)
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

        [Authorize(Roles = "Admin")]
        [HttpGet("{userId}")]
        public IActionResult GetUserById(int userId)
        {
            var user = _userRepository.GetItem(userId);
            if (user == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.Json(null);
            }
            else
            {
                return this.Json(_mapper.Map<UserViewModel>(user));
            }
        }


        [HttpGet("profile")]
        public IActionResult GetUserProfile()
        {
            var user = _userRepository.GetItem(GetUserId());
            if (user == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return this.Json(null);
            }
            else
            {
                return this.Json(_mapper.Map<UserViewModel>(user));
            }
        }


        [HttpPut("{userId}")]
        [ValidateModelState, CheckArgumentsForNull]
        public IActionResult UpdateUser(int userId, [FromBody] UserViewModel userVm)
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
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json(new { Message = $"User with email '{userVm.Email}' already exist." });
            }
            else
            {
                IActionResult result;
                User editedUser = _mapper.Map(userVm, oldUser);

                result = SaveData(() =>
                {
                    _userRepository.Edit(editedUser);
                });
                return result;
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            if(userId == this.GetUserId())
            {
                var user = _userRepository.GetItem(GetUserId());
                var rememberMe = _rememberMeRepository.GetSingleByUserId(user.Id);
                if (rememberMe != null && rememberMe.Selector != null)
                {
                    var remember = _rememberMeRepository.GetSingleByUserId(user.Id);
                    _rememberMeRepository.Delete(remember);
                    _rememberMeRepository.Save();
                }
                await HttpContext.Authentication.SignOutAsync(Startup.AuthenticationScheme);
                return Ok();
            }
            return SaveData(() =>
            {
                _userRepository.Delete(userId);
            });
        }

        
        [HttpPut("changepassword")]    
        public IActionResult ChangePassword([FromBody] ChangePasswordViewModel chgpVM)
        {
           var user = _userRepository.GetItem(x=> x.Id==this.GetUserId());
            if (BCrypt.Net.BCrypt.Verify(chgpVM.oldPassword, user.PasswordHash))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(chgpVM.newPassword);
                user.Password = chgpVM.newPassword;

                _userRepository.Edit(user);
                _userRepository.Save();
                return Ok();
            }
            return NotFound();            
        }


        [HttpPut("changeimage")]
        public IActionResult ChangeImage([FromBody] ChangeImageViewModel chgiVM)
        {
            var user = _userRepository.GetItem(x => x.Id == this.GetUserId());
            user.Photo = Convert.FromBase64String(chgiVM.photoBase64);
                _userRepository.Edit(user);
                _userRepository.Save();
                return Ok();              
        }



        #endregion

        #region Helpers

        private IActionResult CreateNewUser(UserViewModel userVm)
        {
            if (_userRepository.GetItem(u => u.Email == userVm.Email) == null)
            {
                User user = _mapper.Map<User>(userVm);
                user.DateCreated = DateTime.Now;
            //    user.Password

                if (userVm.Photo != null)
                {
                    user.Photo = userVm.Photo;
                }
                else
                {
                    user.Photo = DbInitializer.GetDefaultAvatar();
                }
                var pass = this.GenerateRandomPasswords(8);
                user.Password = pass;

                return SaveData(() =>
                {
                    _userRepository.Add(user);

                },
                () =>
                {
                    _emailService.SendEmail(EmailType.Welcome, user.Email, pass);
                    _emailService.SendEmail(EmailType.PasswordReset, user.Email, pass);
                    this.Response.StatusCode = (int)HttpStatusCode.Created;

                    return this.Json(new JsonResult(this.Json(_mapper.Map<UserViewModel>(user)))
                    {
                        StatusCode = this.Response.StatusCode
                    });
                });
            }
            else
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                return this.Json(new JsonResult($"User with email '{userVm.Email}' already exist.")
                {
                    StatusCode = this.Response.StatusCode
                });
            }
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
                this.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return this.Json(new { Message = $"Saving data throw Exception '{ex.Message}'" });
            }
        }


        public int GetUserId()
        {
            var claims = User.Claims.Select(claim => new { claim.Type, claim.Value }).ToArray();
            var userId = claims[0].Value;
            int id;
            int.TryParse(userId, out id);
            return id;
        }


        public string GenerateRandomPasswords(int length)
        {
            RNGCryptoServiceProvider cryptRNG = new RNGCryptoServiceProvider();
            byte[] tokenBuffer = new byte[length];
            cryptRNG.GetBytes(tokenBuffer);
            return Convert.ToBase64String(tokenBuffer);
        }



        #endregion
    }
}
