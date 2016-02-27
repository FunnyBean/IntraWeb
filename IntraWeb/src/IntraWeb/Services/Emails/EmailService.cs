using IntraWeb.Resources.Email;

using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;
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

        private EmailOptions _options;
        private ILogger<EmailService> _logger;
        private IEmailFormatter _formatter;

        #endregion


        #region Constructor

        public EmailService(IOptions<EmailOptions> options, 
                            ILogger<EmailService> logger, 
                            IEmailFormatter formatter)
        {
            _options = options.Value;
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

                msg.From.Add(new MailboxAddress(Encoding.UTF8, EmailStringTable.MailboxNameFrom, _options.Username));
                msg.To.Add(new MailboxAddress(Encoding.UTF8, EmailStringTable.MailboxNameTo, email));
                msg.Subject = subject;

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
            catch (System.Exception ex)
            {
                _logger.LogError("Error during sending email. Error: ", ex);
            }
        }

    }

}
