using System.Collections.Generic;

namespace IntraWeb.Services.Email
{
    public interface IEmailFormatter
    {

        string FormatEmail(string emailType, IDictionary<string, string> data);

    }
}
