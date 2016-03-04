using MimeKit;


namespace IntraWeb.Services.Email
{
    public static class Extensions
    {

        /// <summary>
        /// Do zoznamu adries <paramref name="addresses" /> pridá adresu <paramref name="email" />
        /// s menom <paramref name="name" />.
        /// </summary>
        /// <param name="addresses">Zoznam adries, do ktorého sa pridá nová adresa.</param>
        /// <param name="email">E-mailová adresa.</param>
        /// <param name="name">Meno adresy, resp. človeka.</param>
        public static void Add(this InternetAddressList addresses, string email, string name)
        {
            addresses.Add(new MailboxAddress(name, email));
        }


        /// <summary>
        /// Do zoznamu adries <paramref name="addresses" /> pridá adresu <paramref name="email" />.
        /// Adresa môže byť aj s názvom, v tvare <c>Alice Wonderland &lt;alice@example.com&gt;</c>.
        /// </summary>
        /// <param name="addresses">Zoznam adries, do ktorého sa pridá nová adresa.</param>
        /// <param name="email">E-mailová adresa.</param>
        public static void Add(this InternetAddressList addresses, string email)
        {
            addresses.Add(MailboxAddress.Parse(email));
        }

    }
}
