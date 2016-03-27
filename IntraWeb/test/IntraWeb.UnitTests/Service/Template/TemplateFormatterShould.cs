using IntraWeb.Services.Template;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace IntraWeb.UnitTests.Service.Template
{
    public class TemplateFormatterShould
    {

        #region Helper classes

        private class UserInfo
        {
            [TemplateVariable("name")]
            public string Name { get; set; }

            [TemplateVariable("lastname")]
            public string Lastname{ get; set; }
        }

        #endregion


        [Fact]
        public void IncludeTemplateInItsLayout()
        {
            var layout = @"<html>
<head><title>Layout template</title></head>
<body>
{include #content}
<body>";

            var template1 = @"{layout TestLayout}
Lorem ipsum dolor sit amet.";

            var template2 = @"{layout ""TestLayout""}
Lorem ipsum dolor sit amet.";

            var template3 = @"{layout 'TestLayout'}
Lorem ipsum dolor sit amet.";

            var expected = @"<html>
<head><title>Layout template</title></head>
<body>

Lorem ipsum dolor sit amet.
<body>";

            var loader = Substitute.For<ITemplateLoader>();
            loader.GetContent("TestLayout").Returns(layout);
            loader.GetContent("Test1").Returns(template1);
            loader.GetContent("Test2").Returns(template2);
            loader.GetContent("Test3").Returns(template3);

            var formatter = new TemplateFormatter(loader);
            var actual1 = formatter.FormatTemplate("Test1", null);
            var actual2 = formatter.FormatTemplate("Test2", null);
            var actual3 = formatter.FormatTemplate("Test3", null);

            Assert.Equal(expected, actual1);
            Assert.Equal(expected, actual2);
            Assert.Equal(expected, actual3);
        }


        [Fact]
        public void ThrowExceptionWhenNoContentMacroInLayout()
        {
            var layout = @"<html>
<head><title>Layout template</title></head>
<body>

<body>";

            var template = @"{layout TestLayout}
Lorem ipsum dolor sit amet.";

            var loader = Substitute.For<ITemplateLoader>();
            loader.GetContent("TestLayout").Returns(layout);
            loader.GetContent("Test").Returns(template);

            var formatter = new TemplateFormatter(loader);
            var ex = Assert.Throws<NoContentInLayoutException>(
                () => {
                    formatter.FormatTemplate("Test", null);
                }
            );
            Assert.Equal("TestLayout", ex.LayoutName);
        }


        [Fact]
        public void IncludeSubTemplates()
        {
            var template1 = @"<p>Paragraph 1.1</p>
{include Template2}
<p>Paragraph 1.2</p>";

            var template2 = @"<p>Paragraph 2.1</p>
{include ""Template3""}
<p>Paragraph 2.2</p>";

            var template3 = @"<p>Paragraph 3.1</p>
{include 'Template4'}
<p>Paragraph 3.2</p>";

            var template4 = @"Lorem ipsum dolor sit amet.";

            var expected = @"<p>Paragraph 1.1</p>
<p>Paragraph 2.1</p>
<p>Paragraph 3.1</p>
Lorem ipsum dolor sit amet.
<p>Paragraph 3.2</p>
<p>Paragraph 2.2</p>
<p>Paragraph 1.2</p>";

            var loader = Substitute.For<ITemplateLoader>();
            loader.GetContent("Template1").Returns(template1);
            loader.GetContent("Template2").Returns(template2);
            loader.GetContent("Template3").Returns(template3);
            loader.GetContent("Template4").Returns(template4);

            var formatter = new TemplateFormatter(loader);
            var actual = formatter.FormatTemplate("Template1", null);

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void LoadDataFromTemplate()
        {
            var template = @"{var $s1 = Lorem ipsum}
{var $s2 = ""Lorem ipsum""}
<p>Lorem ipsum dolor sit amet.</p>
{var $s3 = 'Lorem ipsum'}";

            var expectedTemplate = @"

<p>Lorem ipsum dolor sit amet.</p>
";

            var expectedData = "Lorem ipsum";

            var loader = Substitute.For<ITemplateLoader>();
            loader.GetContent("Test").Returns(template);

            var formatter = new TemplateFormatter(loader);
            var data = new Dictionary<string, object>();
            data["s1"] = "XxX";
            var actualTemplate = formatter.LoadTemplateData(template, data);

            Assert.Equal(expectedTemplate, actualTemplate);
            Assert.Equal(expectedData, data["s1"]);
            Assert.Equal(expectedData, data["s2"]);
            Assert.Equal(expectedData, data["s3"]);
        }


        [Fact]
        public void ThrowArgumentNullExceptionWhenNullTemplateName()
        {
            var loader = Substitute.For<ITemplateLoader>();
            var formatter = new TemplateFormatter(loader);
            Assert.Throws<ArgumentNullException>(() =>
            {
                formatter.FormatTemplate(null, null);
            });
        }


        [Fact]
        public void ThrowArgumentNullExceptionWhenEmptyTemplateName()
        {
            var loader = Substitute.For<ITemplateLoader>();
            var formatter = new TemplateFormatter(loader);
            Assert.Throws<ArgumentNullException>(() =>
            {
                formatter.FormatTemplate(string.Empty, null);
            });
        }


        [Fact]
        public void ThrowArgumentNullExceptionWhenWhiteSpaceTemplateName()
        {
            var loader = Substitute.For<ITemplateLoader>();
            var formatter = new TemplateFormatter(loader);
            Assert.Throws<ArgumentNullException>(() =>
            {
                formatter.FormatTemplate("   ", null);
            });
        }


        [Fact]
        public void ThrowUnknownTemplateVariableException()
        {
            var loader = Substitute.For<ITemplateLoader>();
            loader.GetContent("test").Returns("Lorem {$ipsum} dolor sit amet.");

            var formatter = new TemplateFormatter(loader);

            var ex = Assert.Throws<UnknownTemplateVariableException>(() =>
            {
                formatter.FormatTemplate("test", null);
            });
            Assert.Equal("test", ex.TemplateName);
            Assert.Equal("ipsum", ex.VariableName);
        }


        [Fact]
        public void FormatTemplate()
        {
            var layout = @"<html>
<head>
    <title>{$title}</title>
<body>
    <div id=""header"">{include Header}</div>

{include #content}

    <div id=""footer"">{include Footer}</div>
</body>
</html>";

            var headerTemplate = "<h1>{$title}</h1>";
            var footerTemplate = "Footer - lorem ipsum";

            var template = @"{layout Layout}
{var $title = Lorem ipsum}
<p>User information</p>
<p>Name: {$name}<br />
Lastname: {$lastname}</p>

{include Subtemplate}

<p>End of user information</p>";

            var subTemplate = "This is included subtemplate.";
            var expected = @"<html>
<head>
    <title>Lorem ipsum</title>
<body>
    <div id=""header""><h1>Lorem ipsum</h1></div>



<p>User information</p>
<p>Name: Alice<br />
Lastname: Wonderland</p>

This is included subtemplate.

<p>End of user information</p>

    <div id=""footer"">Footer - lorem ipsum</div>
</body>
</html>";

            var loader = Substitute.For<ITemplateLoader>();
            loader.GetContent("Test").Returns(template);
            loader.GetContent("Layout").Returns(layout);
            loader.GetContent("Header").Returns(headerTemplate);
            loader.GetContent("Footer").Returns(footerTemplate);
            loader.GetContent("Subtemplate").Returns(subTemplate);

            var data = new UserInfo();
            data.Name = "Alice";
            data.Lastname = "Wonderland";
            var formatter = new TemplateFormatter(loader);
            var actual = formatter.FormatTemplate("Test", data);

            Assert.Equal(expected, actual);
        }

    }
}
