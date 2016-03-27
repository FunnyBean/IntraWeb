using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace IntraWeb.Services.Templating
{

    /// <summary>
    /// Implementation of <see cref="ITemplateFormatter">ITemplateFormatter</see> which allows simple formatting of templates.
    /// </summary>
    /// <remarks>
    /// <para>Template usualy consists of two parts. The <b>layout template</b>, which creates general layout of the result.
    /// Its purpose is to make different templates to have the same look. The <b>inner template</b> represents the core part
    /// of template. It is included in layout template. In general, the templates can be included in one another anyhow.</para>
    /// <para>Formatting the template is very simple and it supports following:</para>
    /// <list type="bullet">
    /// <item>Including another template: <c>{include templateName}</c>.</item>
    /// <item>Replacing template variables with data: <c>{$variableName}</c></item>
    /// <item>Setting data directly in template: <c>{var $variableName = value}</c>. Variable name can consist only of english
    /// alphabet characters, numbers, dot and dash. Value can be in single or double quotes, but it is not necessary.</item>
    /// </list>
    /// </remarks>
    public class TemplateFormatter : ITemplateFormatter
    {

        private ITemplateLoader _loader;

        /// <summary>
        /// Initializes a new instance of <see cref="TemplateFormatter">TemplateFormatter</see> with specified template
        /// <paramref name="loader" />.
        /// </summary>
        /// <param name="loader">Template loader.</param>
        public TemplateFormatter(ITemplateLoader loader)
        {
            if (loader == null)
            {
                throw new ArgumentNullException(nameof(loader));
            }
            _loader = loader;
        }


        public const string LayoutTemplateName = "Layout";
        private const string TemplateContentTag = "{include content}";
        private const string TemplateTitleTag = "{$title}";


        /// <summary>
        /// Formats a template <paramref name="templateName" /> and fills it with data from values in <paramref name="data" />.
        /// </summary>
        /// <param name="templateName">Template name.</param>
        /// <param name="data">Data for the template. It can be instance of any class. Data for template variables are values
        /// of properties, which are annotated with <see cref="TemplateVariableAttribute">TemplateVariableAttribute</see>.
        /// <returns>Formatted and filled template.</returns>
        /// <exception cref="ArgumentNullException">Value of <paramref name="templateName" /> is <c>null</c> or empty string
        /// or string consisting of white space characters only.</exception>
        /// <exception cref="UnknownTemplateVariableException">There is a variable in the template, for which there are
        /// no data in <paramref name="data" />.</exception>
        public string FormatTemplate(string templateName, object data)
        {
            if (string.IsNullOrWhiteSpace(templateName))
            {
                throw new ArgumentNullException(nameof(templateName));
            }

            var dataConverter = new TemplateDataConverter();
            var dictionaryData = dataConverter.Convert(data);
            var template = LoadTemplate(templateName, dictionaryData);
            if ((template != null) && (data != null))
            {
                template = FillTemplate(template, dictionaryData, templateName);
            }

            return template;
        }

        /// <summary>
        /// Formats a template <paramref name="templateName" /> and fills it with data from values in <paramref name="data" />.
        /// </summary>
        /// <param name="templateName">Template name.</param>
        /// <param name="data">Data for the template.</param>
        /// <returns>Formatted and filled template.</returns>
        /// <exception cref="ArgumentNullException">Value of <paramref name="templateName" /> is <c>null</c> or empty string
        /// or string consisting of white space characters only.</exception>
        /// <exception cref="UnknownTemplateVariableException">There is a variable in the template, for which there are
        /// no data in <paramref name="data" />.</exception>
        public string FormatTemplate(string templateName, IDictionary<string, object> data)
        {
            if (string.IsNullOrWhiteSpace(templateName))
            {
                throw new ArgumentNullException(nameof(templateName));
            }

            if (data == null)
            {
                data = new Dictionary<string, object>();
            }
            var template = LoadTemplate(templateName, data);
            if ((template != null) && (data != null))
            {
                template = FillTemplate(template, data, templateName);
            }

            return template;
        }


        private string LoadLayout()
        {
            return _loader.GetContent(LayoutTemplateName);
        }


        private string LoadTemplate(string templateName, IDictionary<string, object> data)
        {
            var layout = LoadLayout();
            var template = _loader.GetContent(templateName);
            LoadTemplateData(ref template, data);

            return layout.Replace(TemplateContentTag, template);
        }


        private Regex _reVar = new Regex(@"{var\s+\$(?<name>[a-z0-9.-]+)\s*=\s*(?<quote>""|'|)(?<value>.+?)\k<quote>}",
                                         RegexOptions.IgnoreCase);

        private void LoadTemplateData(ref string template, IDictionary<string, object> data)
        {
            template = _reVar.Replace(template, (m) =>
            {
                data[m.Groups["name"].Value] = m.Groups["value"].Value;
                return string.Empty;
            });
        }


        private Regex _reTemplateKeys = new Regex(@"\{\$(.+?)\}");

        private string FillTemplate(string template, IDictionary<string, object> data, string templateName)
        {
            return _reTemplateKeys.Replace(template,
                (m) =>
                {
                    var key = m.Groups[1].Value;
                    if (data.ContainsKey(key))
                    {
                        return (data[key] == null) ? string.Empty : data[key].ToString();
                    }
                    else
                    {
                        throw new UnknownTemplateVariableException(templateName, key);
                    }
                }
            );
        }

    }

}
