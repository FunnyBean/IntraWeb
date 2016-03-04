using System;

namespace IntraWeb.Services.Emails
{
    public class UnknownEmailTemplateException : Exception
    {

        public UnknownEmailTemplateException(string emailType)
            : base(string.Format(Resources.Resources.UnknownEmailTemplateException_Message, emailType))
        {
            _emailType = emailType;
        }


        private string _emailType;

        public string EmailType { get { return _emailType; } }

    }
}
