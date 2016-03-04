using Microsoft.Extensions.OptionsModel;

using System;

using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;


namespace IntraWeb.Services.Email
{
    public class SmtpEmailSender : IEmailSender
    {


        EmailOptions _options;

        public SmtpEmailSender(IOptions<EmailOptions> options)
        {
            _options = options.Value;
        }


        public void SendEmail(MimeMessage msg)
        {
            if (msg == null) {
                throw new ArgumentNullException(nameof(msg));
            };

            using (var client = new SmtpClient())
            {
                if (_options.UseSsl)
                {
                    client.Connect(_options.Server, _options.Port, SecureSocketOptions.StartTls);
                }
                else
                {
                    client.Connect(_options.Server, _options.Port, false);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                }

                client.Authenticate(_options.Username, _options.Password);
                client.Send(msg);
                client.Disconnect(true);
            }
        }

    }
}
