using Krosbook.Services.Template;
using KrosbookAPI.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Krosbook.Services.Email
{
    public class SendEmailService
    {
        #region private fields
        private IEmailService _emailService;
        private IEmailCreator _creator;
        private IEmailSender _sender;
        #endregion

        #region Constructor
        public SendEmailService(IEmailService emailService,
            IEmailCreator creator,
            IEmailSender sender)
        {
            _emailService = emailService;
            _creator = creator;
            _sender = sender;
        }
        #endregion



        public void SendEmail(EmailType emailType, string to, string password)
        {
            if (emailType == EmailType.Welcome)
            {
                var data = new BaseEmailData(emailType.ToString());
                data.From = Resources.EmailFrom;
                data.To.Add(to);
                var msg = _creator.CreateEmail(data);
                _sender.SendEmail(msg);
            }
            if (emailType == EmailType.PasswordReset)
            {
                var data = new PasswordResetData(@""+password);
                data.From = Resources.EmailFrom;
                data.To.Add(to);
                var msg = _creator.CreateEmail(data);
                _sender.SendEmail(msg);              
            }

            }

    }
}
