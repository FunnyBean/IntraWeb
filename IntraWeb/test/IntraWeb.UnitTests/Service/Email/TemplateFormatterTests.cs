﻿using IntraWeb.Services.Email;
using NSubstitute;
using Xunit;
using Microsoft.AspNet.Hosting;
using System.Collections.Generic;
using System;

namespace IntraWeb.UnitTests.Service.Email
{
    public class TemplateFormatterTests
    {

        #region Input and expected values

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
        {content}
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


        private readonly string _testTitleLayout =
@"<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"" />
    <title>{title}</title>
</head>
<body>
{content}
</body>
</html>";


        private readonly string _testTitleContent =
@"<title>Title inside template</title>
Lorem ipsum";


        private readonly string _expectedTitleTemplate =
@"<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"" />
    <title>Title inside template</title>
</head>
<body>

Lorem ipsum
</body>
</html>";

        #endregion


        [Fact]
        public void ShouldFormatEmail()
        {
            var loader = Substitute.For<ITemplateLoader>();
            loader.GetContent(TemplateFormatter.LayoutTemplateName).Returns(this._layout);
            loader.GetContent("test").Returns(this._testTemplate);

            var formatter = Substitute.For<TemplateFormatter>(loader);

            var data = new Dictionary<string, string>() {
                {"title", "Lorem ipsum"},
                {"user.Name", "Gabriel"},
                {"user.LastName", "Archanjel"}
            };

            var actual = formatter.FormatEmail("test", data);
            Assert.Equal(this._expectedEmail, actual);
        }


        [Fact]
        public void ShouldGetTitleFromTemplate()
        {
            var loader = Substitute.For<ITemplateLoader>();
            loader.GetContent(TemplateFormatter.LayoutTemplateName).Returns(_testTitleLayout);
            loader.GetContent("test").Returns(_testTitleContent);

            var formatter = Substitute.For<TemplateFormatter>(loader);

            var actual = formatter.FormatEmail("test", null);
            Assert.Equal(_expectedTitleTemplate, actual);
        }


        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenNullEmailType()
        {
            var loader = Substitute.For<ITemplateLoader>();
            var formatter = new TemplateFormatter(loader);
            Assert.Throws<ArgumentNullException>(() =>
            {
                formatter.FormatEmail(null, null);
            });
        }


        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenEmptyEmailType()
        {
            var loader = Substitute.For<ITemplateLoader>();
            var formatter = new TemplateFormatter(loader);
            Assert.Throws<ArgumentNullException>(() =>
            {
                formatter.FormatEmail(string.Empty, null);
            });
        }


        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenWhiteSpaceEmailType()
        {
            var loader = Substitute.For<ITemplateLoader>();
            var formatter = new TemplateFormatter(loader);
            Assert.Throws<ArgumentNullException>(() =>
            {
                formatter.FormatEmail("   ", null);
            });
        }


        public void ShouldThrowUnknownKeyExceptionOnInvalidKeyInTemplate()
        {
            var loader = Substitute.For<ITemplateLoader>();
            loader.GetContent(TemplateFormatter.LayoutTemplateName).Returns(this._layout);
            loader.GetContent("test").Returns("Lorem {ipsum} dolor sit amet.");

            var formatter = new TemplateFormatter(loader);

            var ex = Assert.Throws<UnknownKeyException>(() =>
            {
                formatter.FormatEmail("test", null);
            });
            Assert.Equal("test", ex.EmailType);
            Assert.Equal("ipsum", ex.Key);
        }
    }
}
