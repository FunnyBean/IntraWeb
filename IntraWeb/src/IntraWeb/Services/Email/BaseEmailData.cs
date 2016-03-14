using System.Collections.Generic;

namespace IntraWeb.Services.Email
{
    public class BaseEmailData
        : IEmailData
    {

        #region Constructors

        public BaseEmailData() { }

        public BaseEmailData(string emailType)
        {
            this.EmailType = EmailType;
        }

        #endregion


        #region IEmailData

        private List<string> _to = new List<string>();
        private List<string> _cc = new List<string>();
        private List<string> _bcc = new List<string>();

        public string EmailType { get; set; }
        public string From { get; set; }
        public IEnumerable<string> To { get { return _to; } }
        public IEnumerable<string> Cc { get { return _cc; } }
        public IEnumerable<string> Bcc { get { return _bcc; } }
        public string ReplyTo { get; set; }

        #endregion

    }
}
