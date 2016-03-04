using IntraWeb.Services.Emails;
using NSubstitute;
using Xunit;
using Microsoft.AspNet.Hosting;
using System.Collections.Generic;
using System;

namespace IntraWeb.UnitTests.Service.Email
{
    public class FileEmailFormatterTests
    {

        #region Expected values

        private readonly string _layout =
@"<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"" />
    <title>{title}</title>
</head>
<body>
    <div id=""header"">
        <p>
            Suspendisse quis tortor fermentum, volutpat mauris sed, egestas sapien. Nunc at tortor
            nec ante laoreet semper. Mauris non auctor lorem, nec consectetur felis.
        </p>
    </div>
    <div id=""content"">
        {template.content}
    </div>
    <div id=""footer"">
        <p>
            Morbi scelerisque dapibus condimentum.Etiam volutpat erat turpis, quis consectetur
            nisi cursus vitae.
       </p>
    </div>
</body>
</html>";


        private readonly string _testTemplate =
@"<h1>Používateľ</h1>
<p><b>Meno:</b> {user.Name}</p>
<p><b>Priezvisko:</b> {user.LastName}</p>
";


        private readonly string _expectedEmail =
@"<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"" />
    <title>Lorem ipsum</title>
</head>
<body>
    <div id=""header"">
        <p>
            Suspendisse quis tortor fermentum, volutpat mauris sed, egestas sapien. Nunc at tortor
            nec ante laoreet semper. Mauris non auctor lorem, nec consectetur felis.
        </p>
    </div>
    <div id=""content"">
        <h1>Používateľ</h1>
<p><b>Meno:</b> Gabriel</p>
<p><b>Priezvisko:</b> Archanjel</p>

    </div>
    <div id=""footer"">
        <p>
            Morbi scelerisque dapibus condimentum.Etiam volutpat erat turpis, quis consectetur
            nisi cursus vitae.
       </p>
    </div>
</body>
</html>";

        #endregion


        [Fact]
        public void ShouldFormatEmail()
        {
            var env = Substitute.For<IHostingEnvironment>();
            env.WebRootPath.Returns(string.Empty);

            var formatter = Substitute.For<FileEmailFormatter>(env);
            formatter.GetTemplateText(formatter.LayoutTemplateName).Returns(this._layout);
            formatter.GetTemplateText("test").Returns(this._testTemplate);

            var data = new Dictionary<string, string>() {
                {"title", "Lorem ipsum"},
                {"user.Name", "Gabriel"},
                {"user.LastName", "Archanjel"}
            };

            var actual = formatter.FormatEmail("test", data);
            Assert.Equal(this._expectedEmail, actual);
        }


        [Fact]
        public void ShouldReturnNullForInvalidEmailType()
        {
            var env = Substitute.For<IHostingEnvironment>();
            env.WebRootPath.Returns(string.Empty);

            var formatter = new FileEmailFormatter(env);
            var template = formatter.FormatEmail("test", null);
            Assert.Null(template);
        }


        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenNullEmailType()
        {
            var env = Substitute.For<IHostingEnvironment>();
            env.WebRootPath.Returns(string.Empty);

            var formatter = new FileEmailFormatter(env);
            Assert.Throws<ArgumentNullException>(() =>
            {
                formatter.FormatEmail(null, null);
            });
        }


        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenEmptyEmailType()
        {
            var env = Substitute.For<IHostingEnvironment>();
            env.WebRootPath.Returns(string.Empty);

            var formatter = new FileEmailFormatter(env);
            Assert.Throws<ArgumentNullException>(() =>
            {
                formatter.FormatEmail(string.Empty, null);
            });
        }


        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenWhiteSpaceEmailType()
        {
            var env = Substitute.For<IHostingEnvironment>();
            env.WebRootPath.Returns(string.Empty);

            var formatter = new FileEmailFormatter(env);
            Assert.Throws<ArgumentNullException>(() =>
            {
                formatter.FormatEmail("   ", null);
            });
        }

    }
}
