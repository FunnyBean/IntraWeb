using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntraWeb.Services.Email
{
    public class EmailService
        : IEmailService
    {

        #region Private members

        IEmailCreator _creator;
        IEmailSender _sender;

        #endregion


        #region Constructors

        public EmailService(IEmailCreator creator, IEmailSender sender)
        {
            _creator = creator;
            _sender = sender;
        }

        #endregion


        #region Common

        public string EmailFrom { get { return Resources.Resources.EmailFrom;  } }

        #endregion


        #region IEmailService

        public void SendPasswordReset(string to, string resetLink)
        {
            var data = new PasswordResetData(resetLink);
            data.From = EmailFrom;
            data.To.Add(to);

            var msg = _creator.CreateEmail(data);
            _sender.SendEmail(msg);
        }

        #endregion

    }
}
