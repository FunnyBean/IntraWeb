using IntraWeb.Services.Template;
using Xunit;

namespace IntraWeb.UnitTests.Service.Template
{
    public class TemplateDataConverterShould
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
        public void GetPropertyValues()
        {
            var data = new TestData();
            data.TextValue = "Lorem ipsum";
            data.IntValue = 123;

            var converter = new TemplateDataConverter();
            var actual = converter.Convert(data);

            Assert.Equal(2, actual.Count);
            Assert.Equal("Lorem ipsum", actual["Text"]);
            Assert.Equal(123, actual["Int"]);
        }

    }
}
