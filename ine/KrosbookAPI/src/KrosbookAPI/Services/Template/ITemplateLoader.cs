namespace Krosbook.Services.Template
{
    /// <summary>
    /// Represents a mechanism to get a template.
    /// </summary>
    public interface ITemplateLoader
    {
        /// <summary>
        /// Gets a template content for template <paramref name="templateName" />.
        /// </summary>
        /// <param name="templateName">Template name.</param>
        /// <returns>Template content.</returns>
        string GetContent(string templateName);
    }
}
