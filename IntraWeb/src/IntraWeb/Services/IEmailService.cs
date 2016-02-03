using System.Threading.Tasks;

namespace IntraWeb.Services
{
    /// <summary>
    ///  Interface, which describe service for emails.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sending email.
        /// </summary>
        /// <param name="email">The email address "to"</param>
        /// <param name="subject">The subject of email</param>
        /// <param name="message">The message of email</param>
        /// <param name="salutation">The salutation of email</param>
        void SendEmail(string email, string subject, string message, string salutation = null);
    }
}