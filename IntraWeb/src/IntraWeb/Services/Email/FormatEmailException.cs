using System;

namespace IntraWeb.Services.Email
{
    public class FormatEmailException : Exception
    {
        public FormatEmailException()
        {
        }


        public FormatEmailException(string message)
            : base(message)
        {
        }


        public FormatEmailException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }
}
