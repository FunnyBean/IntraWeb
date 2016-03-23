using System.IO;

namespace IntraWeb.Services.Email
{
    public class FileTemplateLoader
        : ITemplateLoader
    {

        public FileTemplateLoader(string rootPath)
        {
            this.RootPath = rootPath;
        }


        public string RootPath { get; }


        public string GetContent(string templateName)
        {
            var templatePath = Path.Combine(RootPath, $"{templateName}.html");
            if (!File.Exists(templatePath)) {
                throw new FileNotFoundException($"Missing template file \"{templatePath}\".", templatePath);
            }
            return File.ReadAllText(templatePath);
        }

    }
}
