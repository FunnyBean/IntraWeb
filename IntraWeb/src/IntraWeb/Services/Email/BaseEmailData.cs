using System.Collections.Generic;

namespace IntraWeb.Services.Email
{
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

        public string EmailType { get; }
        public string From { get; set; }
        public ICollection<string> To { get { return _to; } }
        public ICollection<string> Cc { get { return _cc; } }
        public ICollection<string> Bcc { get { return _bcc; } }
        public string ReplyTo { get; set; }

        #endregion

    }
}
