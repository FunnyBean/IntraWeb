using IntraWeb.Services.Email;
using Xunit;

namespace IntraWeb.UnitTests.Service.Email
{
    public class EmailDataConverterTests
    {

        private class TestData
        {
            public string NotTemplateVar { get; set; } = string.Empty;

            [TemplateVariable("Text")]
            public string TextValue { get; set; }

            [TemplateVariable("Int")]
            public int IntValue{ get; set; }
        }


        [Fact]
        public void ShouldGetPropertyValues()
        {
            var data = new TestData();
            data.TextValue = "Lorem ipsum";
            data.IntValue = 123;

            var converter = new EmailDataConverter();
            var actual = converter.Convert(data);

            Assert.Equal(2, actual.Count);
            Assert.Equal("Lorem ipsum", actual["Text"]);
            Assert.Equal(123, actual["Int"]);
        }

    }
}
