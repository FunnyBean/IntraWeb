using MimeKit;


namespace Krosbook.Services.Email
{
    public static class Extensions
    {

        /// <summary>
        /// To the list of <paramref name="addresses" /> adds address <paramref name="email" />
        /// with name <paramref name="name" />.
        /// </summary>
        /// <param name="addresses">List of addresses where to add new address.</param>
        /// <param name="email">Email address.</param>
        /// <param name="name">Name of the address/person.</param>
        public static void Add(this InternetAddressList addresses, string email, string name)
        {
            addresses.Add(new MailboxAddress(name, email));
        }


        /// <summary>
        /// To the list of <paramref name="addresses" /> adds address <paramref name="email" />.
        /// This can be simple email, or email in the form with person's name <c>Alice Wonderland &lt;alice@example.com&gt;</c>.
        /// </summary>
        /// <param name="addresses">List of addresses where to add new address.</param>
        /// <param name="email">Email address.</param>
        public static void Add(this InternetAddressList addresses, string email)
        {
            addresses.Add(MailboxAddress.Parse(email));
        }

    }
}
