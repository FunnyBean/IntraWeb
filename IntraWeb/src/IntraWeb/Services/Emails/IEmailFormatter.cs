using System.Collections.Generic;

namespace IntraWeb.Services.Emails
{
    public interface IEmailFormatter
    {

        string FormatEmail(string emailType, IDictionary<string, string> data);

    }
}
