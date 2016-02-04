using IntraWeb.Services.Emails;
using MimeKit;
using Xunit;

namespace IntraWeb.UnitTests.Service
{
    /// <summary>
    /// Class for checking EmailFormatter class.
    /// </summary>
    public class EmailFormatterTest
    {

        #region "Validate email template"

        private EmailFormatter CreateEmailFormatter()
        {
            return new EmailFormatter();
        }

        [Fact]
        public void HasEmailTemplateSubject()
        {
            // Arrange
            string subject = "##Email Subject##";
            string body = "##Email Body##";

            // Act
            MimeMessage message = this.CreateEmailFormatter().CreateHTMLEmail(subject, body);

            // Assert            
            Assert.Contains(subject, message.Body.ToString());
        }

        [Fact]
        public void HasEmailTemplateBody()
        {
            // Arrange
            string subject = "##Email Subject##";
            string body = "##Email Body##";

            // Act
            MimeMessage message = this.CreateEmailFormatter().CreateHTMLEmail(subject, body);

            // Assert            
            Assert.Contains(body, message.Body.ToString());
        }

        [Fact]
        public void HasEmailTemplateSalutation()
        {
            // Arrange
            string subject = "##Email Subject##";
            string body = "##Email Body##";
            string salutation = "##Email Salutation##";

            // Act
            MimeMessage message = this.CreateEmailFormatter().CreateHTMLEmail(subject, body, salutation);

            // Assert            
            Assert.Contains(salutation, message.Body.ToString());
        }

        #endregion

    }
}
