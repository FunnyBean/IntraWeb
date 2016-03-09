using Microsoft.AspNet.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace IntraWeb.Services.Email
{

    public class FileEmailFormatter : IEmailFormatter
    {

        private string _templateFolder;

        public FileEmailFormatter(IHostingEnvironment env)
        {
            _templateFolder = Path.Combine(env.WebRootPath, "templates", "email");
        }


        public const string LayoutTemplateName = "Layout";
        public const string TemplateContentTag = "{content}";
        public const string TemplateTitleTag = "{title}";


        public string FormatEmail(string emailType, IDictionary<string, string> data)
        {
            if (string.IsNullOrWhiteSpace(emailType))
            {
                throw new ArgumentNullException(nameof(emailType));
            }

            var template = LoadTemplate(emailType);
            if ((template != null) && (data != null))
            {
                template = FillTemplate(template, data, emailType);
            }

            return template;
        }


        private string LoadLayout()
        {
            return GetTemplateText(LayoutTemplateName);
        }


        private string LoadTemplate(string templateName)
        {
            var layout = LoadLayout();
            var template = GetTemplateText(templateName);
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


        public virtual string GetTemplateText(string templateName)
        {
            var fileName = Path.Combine(_templateFolder, $"{templateName}.html");
            if (!File.Exists(fileName))
            {
                throw new UnknownEmailTemplateException(templateName);
            }
            return File.ReadAllText(fileName);
        }


        private Regex _reTemplateKeys = new Regex(@"\{(.+?)\}");

        private string FillTemplate(string template, IDictionary<string, string> data, string emailType)
        {
            return _reTemplateKeys.Replace(template,
                (m) =>
                {
                    var key = m.Groups[1].Value;
                    if (data.ContainsKey(key))
                    {
                        return data[key];
                    }
                    else
                    {
                        throw new UnknownKeyException(emailType, key);
                    }
                }
            );
        }

    }

}
