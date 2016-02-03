using MimeKit;
using System.Threading.Tasks;

namespace IntraWeb.Services.Emails
{
    /// <summary>
    ///  Interface, which describe email formatter.
    /// </summary>
    public interface IEmailFormatter
    {
        /// <summary>
        /// Creates HTML email.
        /// </summary>
        /// <param name="subject">The subject of email</param>
        /// <param name="message">The message of email</param>
        /// <param name="salutation">The salutation of email</param>
        MimeMessage CreateHTMLEmail(string subject, string message, string salutation = null);
    }
}