using MimeKit;

namespace Krosbook.Services.Email
{

    /// <summary>
    /// Interface for classes that creates an email message.
    /// </summary>
    public interface IEmailCreator
    {

        /// <summary>
        /// Creates an email message with specified <paramref name="data"/>.
        /// </summary>
        /// <param name="data">Data for the message.</param>
        /// <returns>Created email message.</returns>
        MimeMessage CreateEmail(IEmailData data);

    }
}
