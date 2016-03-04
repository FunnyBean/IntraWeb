using Microsoft.AspNet.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace IntraWeb.Services.Emails
{

    public class FileEmailFormatter : IEmailFormatter
    {

        private string _templateFolder;

        public FileEmailFormatter(IHostingEnvironment env)
        {
            _templateFolder = Path.Combine(env.WebRootPath, "templates", "email");
        }


        public string LayoutTemplateName { get; set; } = "Layout";


        public string FormatEmail(string emailType, IDictionary<string, string> data)
        {
            var template = LoadTemplate(emailType);
            template = FillTemplate(template, data);

            return template;
        }


        private string LoadTemplate(string emailType)
        {
            return GetTemplateText(LayoutTemplateName).Replace("{template.content}", GetTemplateText(emailType));
        }


        public virtual string GetTemplateText(string templateName)
        {
            var fileName = Path.Combine(_templateFolder, $"{templateName}.html");
            if (File.Exists(fileName))
            {
                return File.ReadAllText(fileName);
            }

            return string.Empty;
        }


        private Regex _reTemplateKeys = new Regex(@"\{(.+?)\}");

        public string FillTemplate(string template, IDictionary<string, string> data)
        {
            return _reTemplateKeys.Replace(template,
                (m) => data.ContainsKey(m.Groups[1].Value) ? data[m.Groups[1].Value] : m.Value);
        }

    }

}
