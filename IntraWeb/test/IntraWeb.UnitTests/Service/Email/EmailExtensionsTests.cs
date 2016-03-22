using MimeKit;
using Xunit;

using IntraWeb.Services.Email;


namespace IntraWeb.UnitTests.Service.Email
{
    public class EmailExtensionsTests
    {

        [Fact]
        public void ShouldAddEmailAddress()
        {
            var addresses = new InternetAddressList();
            addresses.Add("alice@example.com", "Alice Wonderland");

            var  address = (MailboxAddress)addresses[0];
            Assert.Equal(address.Name, "Alice Wonderland");
            Assert.Equal(address.Address, "alice@example.com");
        }

        [Fact]
        public void ShouldParseEmailAddress()
        {
            var addresses = new InternetAddressList();
            addresses.Add("Alice Wonderland <alice@example.com>");

            var address = (MailboxAddress)addresses[0];
            Assert.Equal(address.Name, "Alice Wonderland");
            Assert.Equal(address.Address, "alice@example.com");
        }

    }
}
