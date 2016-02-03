using IntraWeb.Resources;
using IntraWeb.Resources.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Text;

namespace IntraWeb.Services
{
    /// <summary>
    /// The service for working with emails (sending,..).
    /// This service uses MailKit library for sending email (see more: http://www.mimekit.net/docs/html/Introduction.htm)
    /// </summary>
    public class EmailService : IEmailService
    {

        #region Constants

        public const string cTemplateSubject = "[SUBJECT]";
        public const string cTemplateCompanyWebsite = "[COMPANY_WEBSITE]";
        public const string cTemplateCaption = "[MAIN_CAPTION]";
        public const string cTemplateSalutation = "[SALUTATION]";
        public const string cTemplateBody = "[BODY_TEXT]";
        public const string cTemplateFooter = "[FOOTER_COPYRIGHT]";

        #endregion


        #region Private members

        private ILogger<EmailService> _logger;

        #endregion


        #region Constructor

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        #endregion


        #region Getters/Setters

        public string GetCurrentEmailHTMLTemplate
        {
            get { return EmailHTMLTemplate.HTMLTextResponsive; }
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
                throw new ArgumentException();
            }
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentException();
            }
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException();
            }

            try
            {
                var emailConfigure = this.GetConfiguration();
                var msg = new MimeMessage();
                msg.From.Add(new MailboxAddress(Encoding.UTF8, EmailStringTable.MailboxNameFrom, emailConfigure.userName));
                msg.To.Add(new MailboxAddress(Encoding.UTF8, EmailStringTable.MailboxNameTo, email));
                msg.Subject = subject;
                msg.Body = this.CreateHTMLEmail(message, subject, salutation);

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

        private MimeEntity CreateHTMLEmail(string textMessage, string subject, string salutation)
        {
            var builder = new BodyBuilder();
            
            // Set the plain-text version of the message text
            if (!string.IsNullOrWhiteSpace(salutation))
            {
                builder.TextBody = salutation + "\n\n";
            }
            builder.TextBody += textMessage;

            // Set the html version of the message text
            builder.HtmlBody = this.CreateHTMLBody(textMessage, subject, salutation);

            // Now we just need to set the message body and we're done
            return builder.ToMessageBody();
        }

        private string CreateHTMLBody(string textMessage, string subject, string salutation)
        {
            string ret;

            if (this.ValidateHTMLTemplate(this.GetCurrentEmailHTMLTemplate))
            {
                ret = this.GetCurrentEmailHTMLTemplate; // Template from: http://templates.cakemail.com/details/basic
                ret = ret.Replace(cTemplateSubject, subject);
                ret = ret.Replace(cTemplateCompanyWebsite, EmailStringTable.TemplateCompanyWebSite);
                ret = ret.Replace(cTemplateCaption, string.Empty); // EmailStringTable.TemplateHeaderSubCaption
                ret = ret.Replace(cTemplateSalutation, salutation);
                ret = ret.Replace(cTemplateBody, textMessage.Replace("\n", "<br />"));
                ret = ret.Replace(cTemplateFooter, EmailStringTable.TemplateFooterCopyright);
            }
            else
            {
                throw new Exception("The invalid HTML template.");
            }

            return ret;
        }

        private bool ValidateHTMLTemplate(string htmlTemplate)
        {
            if (string.IsNullOrWhiteSpace(htmlTemplate))
            {
                return false;
            }
            if (!htmlTemplate.Contains(cTemplateSubject))
            {
                return false;
            }
            if (!htmlTemplate.Contains(cTemplateCompanyWebsite))
            {
                return false;
            }
            if (!htmlTemplate.Contains(cTemplateCaption))
            {
                return false;
            }
            if (!htmlTemplate.Contains(cTemplateSalutation))
            {
                return false;
            }
            if (!htmlTemplate.Contains(cTemplateBody))
            {
                return false;
            }
            if (!htmlTemplate.Contains(cTemplateFooter))
            {
                return false;
            }

            return true;
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
