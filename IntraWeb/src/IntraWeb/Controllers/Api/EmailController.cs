using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntraWeb.Services;

namespace IntraWeb.Controllers.Api
{
    [Route("api/email")]
    // Class only for testing
    public class EmailController
    {

        #region Private members

        private IEmailSender _EmailService;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailController"/> class.
        /// </summary>
        /// <param name="emailService">The email service</param>
        public EmailController(IEmailSender emailService)
        {
            _EmailService = emailService;
            _EmailService.SendEmailAsync("majco333@gmail.com", "FunnyBean TEST", "TEST");
        }

        [HttpGet]
        public string Get()
        {
            return "abc";
        }

    }
}
