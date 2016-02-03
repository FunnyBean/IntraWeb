﻿using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntraWeb.Services;
using Microsoft.AspNet.Authorization;
using IntraWeb.Resources.Email;
using IntraWeb.Resources;

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
            _EmailService.SendEmail(email, "FunnyBean Subject TEST", EmailStringTable.TemplateBodyText, EmailStringTable.TemplateBodySalutation);
        }

    }
}
