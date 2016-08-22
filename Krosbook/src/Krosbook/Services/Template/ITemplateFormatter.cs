using System.Collections.Generic;

namespace Krosbook.Services.Template
{
    /// <summary>
    /// Represents a mechanism for formatting templates and filling them with data.
    /// </summary>
    public interface ITemplateFormatter
    {

        /// <summary>
        /// Formats and fills a template <paramref name="templateName" />.
        /// </summary>
        /// <param name="templateName">Template name.</param>
        /// <param name="data">Data for the template. It can be instance of any class. Data for template variables are values
        /// of properties, which are annotated with <see cref="TemplateVariableAttribute">TemplateVariableAttribute</see>.
        /// </param>
        /// <returns>Formatted and filled template.</returns>
        string FormatTemplate(string templateName, object data);

    }
}
