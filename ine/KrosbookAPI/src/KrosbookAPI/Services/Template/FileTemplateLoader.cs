using System.IO;

namespace Krosbook.Services.Template
{
    /// <summary>
    /// Implementation of <see cref="ITemplateLoader">ITemplateLoader</see> which loads templates from HTML files.
    /// </summary>
    /// <remarks>
    /// Templates are HTML files on disk in specified folder. Name of the template file is the name of the template. So when
    /// template name is <b>welcome</b>, template file is <b>welcome.html</b>.
    /// </remarks>
    public class FileTemplateLoader
        : ITemplateLoader
    {

        /// <summary>
        /// Creates a new <c>FileTemplateLoader</c> with defined path for template files.
        /// </summary>
        /// <param name="rootPath">Root path for template files.</param>
        public FileTemplateLoader(string rootPath)
        {
            this.RootPath = rootPath;
        }


        /// <summary>
        /// Root path for template files. All HTML template files are in this folder.
        /// </summary>
        public string RootPath { get; }


        /// <summary>
        /// Gets a content of template <paramref name="templateName" />.
        /// </summary>
        /// <param name="templateName">Template name.</param>
        /// <returns>Content of the template.</returns>
        /// <exception cref="FileNotFoundException">HTML file for template does not exists.</exception>
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
