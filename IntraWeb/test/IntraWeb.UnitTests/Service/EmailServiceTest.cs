using IntraWeb.Services;
using Xunit;

namespace IntraWeb.UnitTests.Service
{
    /// <summary>
    /// Class for checking EmailService class.
    /// </summary>
    public class EmailServiceTest
    {

        #region "Validate email template"

        private EmailService CreateEmailService()
        {
            return new EmailService(new StubLogger<EmailService>());
        }

        [Fact]
        public void HasEmailHTMLTemplateSubject()
        {
            // Assert            
            Assert.Contains(EmailService.cTemplateSubject, this.CreateEmailService().GetCurrentEmailHTMLTemplate);
        }

        [Fact]
        public void HasEmailHTMLTemplateCompanyWebsite()
        {
            // Assert            
            Assert.Contains(EmailService.cTemplateCompanyWebsite, this.CreateEmailService().GetCurrentEmailHTMLTemplate);
        }

        [Fact]
        public void HasEmailHTMLTemplateCaption()
        {
            // Assert            
            Assert.Contains(EmailService.cTemplateCaption, this.CreateEmailService().GetCurrentEmailHTMLTemplate);
        }

        [Fact]
        public void HasEmailHTMLTemplateSalutation()
        {
            // Assert            
            Assert.Contains(EmailService.cTemplateSalutation, this.CreateEmailService().GetCurrentEmailHTMLTemplate);
        }

        [Fact]
        public void HasEmailHTMLTemplateBody()
        {
            // Assert            
            Assert.Contains(EmailService.cTemplateBody, this.CreateEmailService().GetCurrentEmailHTMLTemplate);
        }

        [Fact]
        public void HasEmailHTMLTemplateFooter()
        {
            // Assert            
            Assert.Contains(EmailService.cTemplateFooter, this.CreateEmailService().GetCurrentEmailHTMLTemplate);
        }

        #endregion

    }
}
