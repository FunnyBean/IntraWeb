using System.Collections.Generic;

namespace Krosbook.Services.Email
{
    /// <summary>
    /// Interface for data for emails.
    /// </summary>
    public interface IEmailData
    {

        /// <summary>
        /// Email type. This determines the template, which is used for email.
        /// </summary>
        string EmailType { get; }

        /// <summary>
        /// Email address of sender.
        /// </summary>
        string From { get; }

        /// <summary>
        /// List of email addresses of recepients.
        /// </summary>
        ICollection<string> To { get; }

        /// <summary>
        /// List of email addresses of secondary recepients.
        /// </summary>
        ICollection<string> Cc { get; }

        /// <summary>
        /// List of addresses for BCC recepients.
        /// </summary>
        ICollection<string> Bcc { get; }

        /// <summary>
        /// Email address used when recepient replies to email.
        /// </summary>
        string ReplyTo { get; }

    }
}
