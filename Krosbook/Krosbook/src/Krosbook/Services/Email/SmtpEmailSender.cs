using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;

namespace Krosbook.Services.Email
{
    /// <summary>
    /// Sends email messages using SMTP server.
    /// </summary>
    public class SmtpEmailSender : IEmailSender
    {


        EmailOptions _options;

        /// <summary>
        /// Creates an instance of <c>SmtpEmailSender</c> with options specified in <paramref name="options" />.
        /// </summary>
        /// <param name="options">Options for the sender.</param>
        public SmtpEmailSender(IOptions<EmailOptions> options)
        {
            _options = options.Value;
        }


        /// <summary>
        /// Sends an email <paramref name="msg" />.
        /// </summary>
        /// <param name="msg">Message to send.</param>
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
