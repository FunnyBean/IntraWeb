using IntraWeb.Resources;
using IntraWeb.Resources.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Text;

namespace IntraWeb.Services.Emails
{
    /// <summary>
    /// The service for working with emails (sending,..).
    /// This service uses MailKit library for sending email (see more: http://www.mimekit.net/docs/html/Introduction.htm)
    /// </summary>
    public class EmailService : IEmailService
    {

        #region Private members

        private ILogger<EmailService> _logger;
        private IEmailFormatter _formatter;

        #endregion


        #region Constructor

        public EmailService(ILogger<EmailService> logger,
                                  IEmailFormatter formatter)
        {
            _logger = logger;
            _formatter = formatter;
        }

        #endregion


        /// <summary>
        /// Sending email
        /// </summary>
        /// <param name="email">The email address "to".</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="message">The message of the email.</param>
        /// <param name="salutation">Optional: The salutation of the email.</param>
        public void SendEmail(string email, string subject, string message, string salutation = null)
        {
            // Validate arguments
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException($"Argument {nameof(email) } is required.");
            }
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentException($"Argument {nameof(subject) } is required.");
            }
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException($"Argument {nameof(message) } is required.");
            }

            try
            {
                var msg = _formatter.CreateHTMLEmail(subject, message, salutation);

                var emailConfigure = this.GetConfiguration();
                msg.From.Add(new MailboxAddress(Encoding.UTF8, EmailStringTable.MailboxNameFrom, emailConfigure.userName));
                msg.To.Add(new MailboxAddress(Encoding.UTF8, EmailStringTable.MailboxNameTo, email));
                msg.Subject = subject;

                using (var client = new SmtpClient())
                {
                    if (emailConfigure.useSsl)
                    {
                        client.Connect(emailConfigure.smtp, emailConfigure.port, SecureSocketOptions.StartTls);
                    }
                    else
                    {
                        client.Connect(emailConfigure.smtp, emailConfigure.port, false);
                        client.AuthenticationMechanisms.Remove("XOAUTH2");

                    }

                    client.Authenticate(emailConfigure.userName, emailConfigure.password);
                    client.Send(msg);
                    client.Disconnect(true);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError("Error during sending email. Error: ", ex);
            }
        }

        private EmailConfiguration GetConfiguration()
        {
            EmailConfiguration ret = new EmailConfiguration();
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            builder.AddEnvironmentVariables();
            var config = builder.Build();

#if DEBUG
            ret.smtp = config["Email:Gmail_SMTP:serverAddress"];
            ret.port = int.Parse(config["Email:Gmail_SMTP:port"]);
            ret.useSsl = bool.Parse(config["Email:Gmail_SMTP:useSsl"]);
            ret.userName = config["Email:Gmail_SMTP:userName"];
            ret.password = config["Email:Gmail_SMTP:password"];
#else
            ret.smtp = config["Email:SendGrid_SMTP:serverAddress"];
            ret.port = int.Parse(config["Email:SendGrid_SMTP:port"]);
            ret.useSsl = bool.Parse(config["Email:SendGrid_SMTP:useSsl"]);
            ret.userName = config["Email:SendGrid_SMTP:userName"];
            ret.password = config["Email:SendGrid_SMTP:password"];
#endif

            return ret;
        }



        /// <summary>
        /// The scructure for email's configuration
        /// </summary>
        private struct EmailConfiguration
        {
            public string smtp;
            public int port;
            public bool useSsl;
            public string userName;
            public string password;
        }

    }


}
