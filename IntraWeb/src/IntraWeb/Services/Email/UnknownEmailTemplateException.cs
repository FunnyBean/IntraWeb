using System;

namespace IntraWeb.Services.Emails
{
    public class UnknownEmailTemplateException : Exception
    {

        public UnknownEmailTemplateException(string emailType)
            : base($"Emailová šablóna \"{emailType}\" neexistuje.")
        {
            _emailType = emailType;
        }


        private string _emailType;

        public string EmailType { get { return _emailType; } }

    }
}
