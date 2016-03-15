using IntraWeb.Services.Email;
using Microsoft.AspNet.Hosting;
using MimeKit;
using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace IntraWeb.UnitTests.Service.Email
{
    public class HtmlEmailCreatorTests
    {

        private readonly string _htmlBody =
@"<DOCTYPE html>
<html>
<head>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">
    <title>Lorem ipsum</title>
</head>
<body>
   <h1>Main Header</h1>
   <p>Paragraph 1.
Second line with link: <a href=""http://example.com"">link to example</a>.
Third line: <a href='http://example.com'>link to example</a>.
Last line.  </p>

<p>Paragraph 2.</p>
</body>";

        private readonly string _textBody =
@"Main Header

Paragraph 1. Second line with link: link to example (http://example.com). Third line: link to example (http://example.com). Last line.

Paragraph 2.";


        [Fact]
        public void ShouldCreateMessage()
        {
            const string emailType = "test";

            var env = Substitute.For<IHostingEnvironment>();
            env.WebRootPath.Returns(string.Empty);
            var formatter = Substitute.For<ITemplateFormatter>();
            formatter.FormatTemplate(emailType, Arg.Any<IDictionary<string, string>>()).Returns(_htmlBody);

            var creator = new HtmlEmailCreator(env, formatter);
            var msg = creator.CreateEmail(emailType, new BaseEmailData());

            Assert.Equal("Lorem ipsum", msg.Subject);

            TextPart htmlPart = null;
            TextPart textPart = null;
            foreach (var part in msg.BodyParts)
            {
                if (part.ContentType.IsMimeType("text", "html"))
                {
                    htmlPart = part as TextPart;
                }
                else if (part.ContentType.IsMimeType("text", "plain"))
                {
                    textPart = part as TextPart;
                }
            }

            Assert.Equal(_htmlBody, htmlPart.Text);
            Assert.Equal(_textBody, textPart.Text);
        }


        [Fact]
        public void ShouldHaveCorrectAddresses()
        {
            const string emailType = "test";

            var env = Substitute.For<IHostingEnvironment>();
            env.WebRootPath.Returns(string.Empty);
            var formatter = Substitute.For<ITemplateFormatter>();
            formatter.FormatTemplate(emailType, Arg.Any<IDictionary<string, string>>()).Returns(_htmlBody);

            var creator = new HtmlEmailCreator(env, formatter);

            var data = new BaseEmailData();
            data.From = "From Email <from@example.com>";
            data.To.Add("To Email <to@example.com>");
            data.Cc.Add("Cc Email <cc@example.com>");
            data.Bcc.Add("Bcc Email <bcc@example.com>");
            data.ReplyTo = "ReplyTo Email <replyto@example.com>";
            var msg = creator.CreateEmail(emailType, data);

            var emailAddress = msg.From[0] as MailboxAddress;
            Assert.Equal(emailAddress.Name, "From Email");
            Assert.Equal(emailAddress.Address, "from@example.com");

            emailAddress = msg.To[0] as MailboxAddress;
            Assert.Equal(emailAddress.Name, "To Email");
            Assert.Equal(emailAddress.Address, "to@example.com");

            emailAddress = msg.Cc[0] as MailboxAddress;
            Assert.Equal(emailAddress.Name, "Cc Email");
            Assert.Equal(emailAddress.Address, "cc@example.com");

            emailAddress = msg.Bcc[0] as MailboxAddress;
            Assert.Equal(emailAddress.Name, "Bcc Email");
            Assert.Equal(emailAddress.Address, "bcc@example.com");

            emailAddress = msg.ReplyTo[0] as MailboxAddress;
            Assert.Equal(emailAddress.Name, "ReplyTo Email");
            Assert.Equal(emailAddress.Address, "replyto@example.com");
        }

    }
}
