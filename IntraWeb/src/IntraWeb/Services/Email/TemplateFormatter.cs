using Microsoft.AspNet.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace IntraWeb.Services.Email
{

    public class TemplateFormatter : ITemplateFormatter
    {

        private ITemplateLoader _loader;

        public TemplateFormatter(ITemplateLoader loader)
        {
            if (loader == null)
            {
                throw new ArgumentNullException(nameof(loader));
            }
            _loader = loader;
        }


        public const string LayoutTemplateName = "Layout";
        public const string TemplateContentTag = "{include content}";
        public const string TemplateTitleTag = "{$title}";


        public string FormatTemplate(string templateName, IDictionary<string, object> data)
        {
            if (string.IsNullOrWhiteSpace(templateName))
            {
                throw new ArgumentNullException(nameof(templateName));
            }

            var template = LoadTemplate(templateName);
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


        private string LoadTemplate(string templateName)
        {
            var layout = LoadLayout();
            var template = _loader.GetContent(templateName);
            SetTemplateHtmlTitle(ref layout, ref template);

            return layout.Replace(TemplateContentTag, template);
        }


        private Regex _reTitle = new Regex("<title>(.+?)</title>", RegexOptions.IgnoreCase);

        private void SetTemplateHtmlTitle(ref string layout, ref string template)
        {
            string title = null;
            template = _reTitle.Replace(template, (m) =>
            {
                title = m.Groups[1].Value;
                return string.Empty;
            });
            if (title != null)
            {
                layout = layout.Replace(TemplateTitleTag, title);
            }
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
                        throw new UnknownKeyException(templateName, key);
                    }
                }
            );
        }

    }

}
