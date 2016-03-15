using System.Collections.Generic;

namespace IntraWeb.Services.Email
{
    public interface ITemplateFormatter
    {

        string FormatTemplate(string templateName, IDictionary<string, string> data);

    }
}
