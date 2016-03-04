using System;

namespace IntraWeb.Services.Emails
{
    public class CreateEmailException : Exception
    {

        public CreateEmailException(string emailType)
            : base ($"Nepodarilo sa vytvoriť správu pre typ emailu \"{emailType}\".")
        {
            _emailType = emailType;
        }


        private string _emailType;

        public string EmailType { get { return _emailType;  } }

    }
}
