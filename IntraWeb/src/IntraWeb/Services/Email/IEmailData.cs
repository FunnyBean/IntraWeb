using System.Collections.Generic;

namespace IntraWeb.Services.Email
{
    public interface IEmailData
    {

        string EmailType { get; }

        string From { get; }
        ICollection<string> To { get; }
        ICollection<string> Cc { get; }
        ICollection<string> Bcc { get; }
        string ReplyTo { get; }

    }
}
