using IntraWeb.Resources.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using System.Text;

namespace IntraWeb.Services
{
    /// <summary>
    /// The service for working with emails (sending,..).
    /// This service uses MailKit library for sending email (see more: http://www.mimekit.net/docs/html/Introduction.htm)
    /// </summary>
    public class EmailService : IEmailService
    {

        #region Private members

        private ILogger<EmailService> _logger;

        #endregion


        #region Constructor

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        #endregion

        public void SendEmail(string email, string subject, string message, string salutation)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(email))
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
            builder.HtmlBody = EmailHTMLTemplate.HTMLTextResponsive; // Template from: http://templates.cakemail.com/details/basic
            builder.HtmlBody = builder.HtmlBody.Replace("[SUBJECT]", subject);
            builder.HtmlBody = builder.HtmlBody.Replace("[CLIENTS_WEBSITE]", EmailStringTable.TemplateCompanyWebSite);
            builder.HtmlBody = builder.HtmlBody.Replace("[MAIN_CAPTION]", string.Empty); // EmailStringTable.TemplateHeaderSubCaption
            builder.HtmlBody = builder.HtmlBody.Replace("[SALUTATION]", salutation);
            builder.HtmlBody = builder.HtmlBody.Replace("[BODY_TEXT]", textMessage.Replace("\n", "<br />"));
            builder.HtmlBody = builder.HtmlBody.Replace("[FOOTER_COPYRIGHT]", EmailStringTable.TemplateFooterCopyright);

            // Now we just need to set the message body and we're done
            return builder.ToMessageBody();
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
