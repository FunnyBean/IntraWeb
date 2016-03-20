using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using IntraWeb.Services.Email;
using System.Collections.Generic;

namespace IntraWeb.Controllers.Api.v1
{

    [Route("api/email")]
    [AllowAnonymous]
    public class EmailController : BaseController
    {

        #region Private members

        private IEmailCreator _creator;
        private IEmailSender _sender;

        #endregion


        public EmailController(IEmailCreator creator, IEmailSender sender)
        {
            _creator = creator;
            _sender = sender;
        }


        [HttpGet]
        [Route("send/{emailType}")]
        public void Send(string emailType, string to)
        {
            var data = new BaseEmailData(emailType);
            data.From = Resources.Resources.EmailFrom;
            data.To.Add(to);

            var msg = _creator.CreateEmail(data);
            _sender.SendEmail(msg);
        }

        [HttpGet]
        [Route("PasswordReset")]
        public void PasswordReset(string to)
        {
            var data = new PasswordResetData(@"http://example.com");
            data.From = Resources.Resources.EmailFrom;
            data.To.Add(to);

            var msg = _creator.CreateEmail(data);
            _sender.SendEmail(msg);
        }

    }
}
