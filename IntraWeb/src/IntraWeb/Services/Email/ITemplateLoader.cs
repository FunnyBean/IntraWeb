namespace IntraWeb.Services.Email
{
    public interface ITemplateLoader
    {
        string GetContent(string templateName);
    }
}
