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
    public class EmailFormatter : IEmailFormatter
    {

        #region Constants

        private const string cTemplateSubject = "[SUBJECT]";
        private const string cTemplateCompanyWebsite = "[COMPANY_WEBSITE]";
        private const string cTemplateCaption = "[MAIN_CAPTION]";
        private const string cTemplateSalutation = "[SALUTATION]";
        private const string cTemplateBody = "[BODY_TEXT]";
        private const string cTemplateFooter = "[FOOTER_COPYRIGHT]";

        #endregion


        /// <summary>
        /// Creates HTML email.
        /// </summary>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="message">The message of the email.</param>
        /// <param name="salutation">Optional: The salutation of the email.</param>
        public MimeMessage CreateHTMLEmail(string subject, string message, string salutation = null)
        {
            MimeMessage ret = new MimeMessage();

            // Validate arguments
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentException($"Argument {nameof(subject) } is required.");
            }
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException($"Argument {nameof(message) } is required.");
            }

            ret.Body = this.CreateHTMLEmailBody(message, subject, salutation);

            return ret;
        }

        private MimeEntity CreateHTMLEmailBody(string textMessage, string subject, string salutation)
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

            if (this.ValidateHTMLTemplate(EmailHTMLTemplate.HTMLTextResponsive))
            {
                ret = EmailHTMLTemplate.HTMLTextResponsive; // Template from: http://templates.cakemail.com/details/basic
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

    }

}
