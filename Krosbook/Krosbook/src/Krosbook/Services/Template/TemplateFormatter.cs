using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Krosbook.Services.Template
{

    /// <summary>
    /// Implementation of <see cref="ITemplateFormatter">ITemplateFormatter</see> which allows simple formatting of templates.
    /// </summary>
    /// <remarks>
    /// <para>Templates are formatted with <b>macros</b>, which are special keywords in curly braces. Macros usually have
    /// some arguments.</para>
    /// <para>Data in templates are inserted using <c>{$variableName}</c> macro. This construct is replaced with the value
    /// of variable with corresponding name. So <c>{$username}</c> is replaced with the value of data with name <c>username</c>.
    /// Values for variables can be defined directly in template using macro <c>{var}</c>. Syntax of this macro is
    /// <c>{var $title = Lorem ipsum}</c>. This way is created a variable with name <c>title</c> and value <c>Lorem ipsum</c>.
    /// Value can be quoted in double or single quotes, but it is not necessary.</para>
    /// <para>Templates can be nested to arbitrary depth. Macro for nesting templates is <c>{include templateName}</c>.
    /// This macro is replaced with content of template <c>templateName</c>.</para>
    /// <para>Special case of template nesting is <b>layout template</b>. Layout template defines layout for several other
    /// templates, so these templates have the same look. layout of the template is defined with macro
    /// <c>{layout templateName}</c>. When it is found, template <c>templateName</c> is loaded and current template is inserted
    /// in it. Special macro <c>{include #content}</c> is used for insertion, so it needs to be somewhere in layout template.
    /// </para>
    /// </remarks>
    public class TemplateFormatter : ITemplateFormatter
    {

        private ITemplateLoader _loader;

        /// <summary>
        /// Initializes a new instance of <see cref="TemplateFormatter">TemplateFormatter</see> with specified template loader.
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


        /// <summary>
        /// Formats a template <paramref name="templateName" /> and fills it with data from values in <paramref name="data" />.
        /// </summary>
        /// <param name="templateName">Template name.</param>
        /// <param name="data">Data for the template. It can be instance of any class. Data for template variables are values
        /// of properties, which are annotated with <see cref="TemplateVariableAttribute">TemplateVariableAttribute</see>.
        /// <returns>Formatted and filled template.</returns>
        /// <exception cref="ArgumentNullException">Value of <paramref name="templateName" /> is <c>null</c> or empty string
        /// or string consisting of white space characters only.</exception>
        /// <exception cref="NoContentInLayoutException">Template has defined its layout using macro <c>{layout templateName}</c>,
        /// but there is no content macro <c>{include #content}</c> in layout template. </exception>
        /// <exception cref="UnknownTemplateVariableException">There is a variable in the template, for which there are
        /// no data in <paramref name="data" />.</exception>
        public string FormatTemplate(string templateName, object data)
        {
            if (string.IsNullOrWhiteSpace(templateName))
            {
                throw new ArgumentNullException(nameof(templateName));
            }

            var dataConverter = new TemplateDataConverter();
            var dictionaryData = (data == null) ? new Dictionary<string, object>() : dataConverter.Convert(data);
            var template = LoadTemplate(templateName, dictionaryData);
            if (template != null)
            {
                template = FillTemplate(template, dictionaryData, templateName);
            }

            return template;
        }


        private string LoadTemplate(string templateName, IDictionary<string, object> data)
        {
            var template = LoadTemplateCore(templateName);
            template = IncludeSubTemplates(template);
            template = LoadTemplateData(template, data);

            return template;
        }


        private Regex _reLayout = new Regex(@"\{layout\s+(?<quote>""|'|)(?<name>.+?)\k<quote>\}", RegexOptions.IgnoreCase);

        private string LoadTemplateCore(string templateName)
        {
            string layoutName = null;
            string layout = null;
            var content = _loader.GetContent(templateName);
            if (!string.IsNullOrEmpty(content))
            {
                content = _reLayout.Replace(content,
                    (m) =>
                    {
                        layoutName = m.Groups["name"].Value;
                        layout = _loader.GetContent(layoutName);
                        return string.Empty;
                    });
            }
            if (!string.IsNullOrEmpty(layout))
            {
                content = FillLayoutWithContent(layoutName, layout, content);
            }

            return content;
        }


        private Regex _reLayoutContent = new Regex(@"\{include\s+#content\}", RegexOptions.IgnoreCase);

        private string FillLayoutWithContent(string layoutName, string layout, string content)
        {
            var contentReplaced = false;
            content = _reLayoutContent.Replace(layout,
                (m) =>
                {
                    contentReplaced = true;
                    return content;
                },
                1);
            if (!contentReplaced)
            {
                throw new NoContentInLayoutException(layoutName);
            }
            return content;
        }


        private Regex _reInclude = new Regex(@"\{include\s+(?<quote>""|'|)(?<name>.+?)\k<quote>\}", RegexOptions.IgnoreCase);

        private string IncludeSubTemplates(string template)
        {
            return _reInclude.Replace(template,
                (m) =>
                {
                    var subTemplate = _loader.GetContent(m.Groups["name"].Value);
                    subTemplate = IncludeSubTemplates(subTemplate);
                    return subTemplate;
                });
        }


        private Regex _reVar = new Regex(@"\{var\s+\$(?<name>[a-z0-9.-]+)\s*=\s*(?<quote>""|'|)(?<value>.+?)\k<quote>\}",
                                         RegexOptions.IgnoreCase);

        public string LoadTemplateData(string template, IDictionary<string, object> data)
        {
            template = _reVar.Replace(template,
                (m) =>
                {
                    data[m.Groups["name"].Value] = m.Groups["value"].Value;
                    return string.Empty;
                });
            return template;
        }


        private Regex _reTemplateKeys = new Regex(@"\{\$(?<varName>.+?)\}");

        private string FillTemplate(string template, IDictionary<string, object> data, string templateName)
        {
            return _reTemplateKeys.Replace(template,
                (m) =>
                {
                    var key = m.Groups["varName"].Value;
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
