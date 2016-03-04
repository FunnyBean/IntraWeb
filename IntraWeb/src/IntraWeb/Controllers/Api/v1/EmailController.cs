using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using IntraWeb.Services.Emails;

namespace IntraWeb.Controllers.Api.v1
{
    /// <summary>
    /// Class only for testing sending emails.
    /// </summary>
    [Route("api/email")]
    [AllowAnonymous]
    public class EmailController : BaseController
    {

        #region Private members

        private IEmailService _EmailService;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailController"/> class.
        /// </summary>
        /// <param name="emailService">The email service</param>
        public EmailController(IEmailService emailService)
        {
            _EmailService = emailService;

        }

        /// <summary>
        /// Test sending email.
        /// </summary>
        [HttpGet("{email}")]
        public void TestSendingEmail(string email)
        {
            _EmailService.SendEmail(email, "FunnyBean Subject TEST", "", "");
        }

    }
}
