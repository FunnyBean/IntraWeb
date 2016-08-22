using MimeKit;

namespace Krosbook.Services.Email
{
    /// <summary>
    /// Defines an interface for sending email messages.
    /// </summary>
    public interface IEmailSender
    {

        /// <summary>
        /// Sends an email <paramref name="msg" />.
        /// </summary>
        /// <param name="msg">Message to send.</param>
        void SendEmail(MimeMessage msg);

    }
}
