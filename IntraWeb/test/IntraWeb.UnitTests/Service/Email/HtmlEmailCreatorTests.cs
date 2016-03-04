using IntraWeb.Services.Emails;
using MimeKit;
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
Last line.  </p>

<p>Paragraph 2.</p>
</body>";

        private readonly string _textBody =
@"Main Header

Paragraph 1. Second line with link: link to example (http://example.com). Last line.

Paragraph 2.";


        [Fact]
        public void ShouldCreateMessage()
        {
            var creator = new HtmlEmailCreator();
            var msg = creator.CreateEmail(_htmlBody);

            Assert.Equal("Lorem ipsum", msg.Subject);

            var body = msg.Body as MultipartAlternative;
            Assert.Equal(_htmlBody, body.HtmlBody);
            Assert.Equal(_textBody, body.TextBody);
        }

    }
}
