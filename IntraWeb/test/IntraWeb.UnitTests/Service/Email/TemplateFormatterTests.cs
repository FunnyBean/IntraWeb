using IntraWeb.Services.Email;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

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
    <title>{$title}</title>
</head>
<body>
    <div id=""header"">
        <p>
            Suspendisse quis tortor fermentum, volutpat mauris sed, egestas sapien. Nunc at tortor
            nec ante laoreet semper. Mauris non auctor lorem, nec consectetur felis.
        </p>
    </div>
    <div id=""content"">
        {include content}
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
<p><b>Meno:</b> {$user.Name}</p>
<p><b>Priezvisko:</b> {$user.LastName}</p>
<p><b>Vek:</b> {$user.Age}</p>
";


        private readonly string _expectedTemplate =
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
<p><b>Vek:</b> 37</p>

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
    <title>{$title}</title>
</head>
<body>
{include content}
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
        public void ShouldFormatTemplate()
        {
            var loader = Substitute.For<ITemplateLoader>();
            loader.GetContent(TemplateFormatter.LayoutTemplateName).Returns(this._layout);
            loader.GetContent("test").Returns(this._testTemplate);

            var formatter = Substitute.For<TemplateFormatter>(loader);

            var data = new Dictionary<string, object>() {
                {"title", "Lorem ipsum"},
                {"user.Name", "Gabriel"},
                {"user.LastName", "Archanjel"},
                {"user.Age", 37}
            };

            var actual = formatter.FormatTemplate("test", data);
            Assert.Equal(this._expectedTemplate, actual);
        }


        [Fact]
        public void ShouldGetTitleFromTemplate()
        {
            var loader = Substitute.For<ITemplateLoader>();
            loader.GetContent(TemplateFormatter.LayoutTemplateName).Returns(_testTitleLayout);
            loader.GetContent("test").Returns(_testTitleContent);

            var formatter = Substitute.For<TemplateFormatter>(loader);

            var actual = formatter.FormatTemplate("test", null);
            Assert.Equal(_expectedTitleTemplate, actual);
        }


        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenNullTemplateName()
        {
            var loader = Substitute.For<ITemplateLoader>();
            var formatter = new TemplateFormatter(loader);
            Assert.Throws<ArgumentNullException>(() =>
            {
                formatter.FormatTemplate(null, null);
            });
        }


        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenEmptyTemplateName()
        {
            var loader = Substitute.For<ITemplateLoader>();
            var formatter = new TemplateFormatter(loader);
            Assert.Throws<ArgumentNullException>(() =>
            {
                formatter.FormatTemplate(string.Empty, null);
            });
        }


        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenWhiteSpaceTemplateName()
        {
            var loader = Substitute.For<ITemplateLoader>();
            var formatter = new TemplateFormatter(loader);
            Assert.Throws<ArgumentNullException>(() =>
            {
                formatter.FormatTemplate("   ", null);
            });
        }


        public void ShouldThrowUnknownKeyExceptionOnInvalidKeyInTemplate()
        {
            var loader = Substitute.For<ITemplateLoader>();
            loader.GetContent(TemplateFormatter.LayoutTemplateName).Returns(this._layout);
            loader.GetContent("test").Returns("Lorem {$ipsum} dolor sit amet.");

            var formatter = new TemplateFormatter(loader);

            var ex = Assert.Throws<UnknownKeyException>(() =>
            {
                formatter.FormatTemplate("test", null);
            });
            Assert.Equal("test", ex.TemplateName);
            Assert.Equal("ipsum", ex.Key);
        }
    }
}
