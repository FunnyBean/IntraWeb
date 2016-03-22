using System.Collections.Generic;

namespace IntraWeb.Services.Email
{
    public interface ITemplateFormatter
    {

        string FormatEmail(string templateName, IDictionary<string, string> data);

    }
}
