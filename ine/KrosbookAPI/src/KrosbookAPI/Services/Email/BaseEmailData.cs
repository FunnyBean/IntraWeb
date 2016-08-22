using Krosbook.Services.Template;
using System.Collections.Generic;

namespace Krosbook.Services.Email
{

    /// <summary>
    /// Base data for emails.
    /// </summary>
    /// <remarks>
    /// <para>For emails which do not require additional data, instance of this class can be used. For emails requiring data
    /// this class should be inheritedand. The data will be strongly typed properties, annotated with
    /// <see cref="IntraWeb.Services.Template.TemplateVariableAttribute"/>TemplateVariableAttribute</para>.
    /// </remarks>
    public class BaseEmailData
        : IEmailData
    {

        #region Constructors

        public BaseEmailData(string emailType)
        {
            this.EmailType = emailType;    
        }

        #endregion


        #region IEmailData

        private List<string> _to = new List<string>();
        private List<string> _cc = new List<string>();
        private List<string> _bcc = new List<string>();

        /// <summary>
        /// Email type. This determines the template, which is used for email.
        /// </summary>
        public string EmailType { get; }

        /// <summary>
        /// Email address of sender.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// List of email addresses of recepients.
        /// </summary>
        public ICollection<string> To { get { return _to; } }

        /// <summary>
        /// List of email addresses of secondary recepients.
        /// </summary>
        public ICollection<string> Cc { get { return _cc; } }

        /// <summary>
        /// List of addresses for BCC recepients.
        /// </summary>
        public ICollection<string> Bcc { get { return _bcc; } }

        /// <summary>
        /// Email address used when recepient replies to email.
        /// </summary>
        public string ReplyTo { get; set; }


        #endregion

    }
}
