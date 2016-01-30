using IntraWeb.Resources.Email;
using MailKit.Net.Smtp;
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

        public void SendEmail(string email, string subject, string message)
        {
            try
            {
                var builder = new ConfigurationBuilder();
                builder.AddJsonFile("appsettings.json");
                builder.AddEnvironmentVariables();
                var config = builder.Build();

                string smtp = config["Email:SMTP:serverAddress"];
                int port = int.Parse(config["Email:SMTP:port"]);
                bool useSsl = bool.Parse(config["Email:SMTP:useSsl"]);
                string userName = config["Email:SMTP:userName"];
                string password = config["Email:SMTP:password"];

                if (!string.IsNullOrWhiteSpace(email))
                {
                    var msg = new MimeMessage();
                    msg.From.Add(new MailboxAddress(Encoding.UTF8, EmailStringTable.MailboxNameFrom, userName));
                    msg.To.Add(new MailboxAddress(Encoding.UTF8, EmailStringTable.MailboxNameTo, email));
                    msg.Subject = subject;
                    msg.Body = this.CreateHTMLEmail(message);

                    using (var client = new SmtpClient())
                    {
                        client.Connect(smtp, port, useSsl);

                        if (useSsl)
                        {
                            client.Authenticate(userName, password);
                        }

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

        private MimeEntity CreateHTMLEmail(string textMessage)
        {
            var builder = new BodyBuilder();
            
            // Set the plain-text version of the message text
            builder.TextBody = textMessage;

            // Set the html version of the message text
            builder.HtmlBody = EmailHTMLTemplate.HTMLText;
            builder.HtmlBody = builder.HtmlBody.Replace("##HEADER_CAPTION##", EmailStringTable.TemplateHeaderCaption);
            builder.HtmlBody = builder.HtmlBody.Replace("##HEADER_SUB_CAPTION##", EmailStringTable.TemplateHeaderSubCaption);
            builder.HtmlBody = builder.HtmlBody.Replace("##BODY_SALUTATION##", EmailStringTable.TemplateBodySalutation);
            builder.HtmlBody = builder.HtmlBody.Replace("##BODY_TEXT##", textMessage.Replace("\n", "<br />"));
            builder.HtmlBody = builder.HtmlBody.Replace("##FOOTER_COPYRIGHT##", EmailStringTable.TemplateFooterCopyright);

            // Now we just need to set the message body and we're done
            return builder.ToMessageBody();
        }
    }
}
