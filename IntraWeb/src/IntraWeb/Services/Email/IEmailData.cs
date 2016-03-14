using System.Collections.Generic;

namespace IntraWeb.Services.Email
{
    public interface IEmailData
    {

        string EmailType { get; set; }

        string From { get; }
        IEnumerable<string> To { get; }
        IEnumerable<string> Cc { get; }
        IEnumerable<string> Bcc { get; }
        string ReplyTo { get; }

    }
}
