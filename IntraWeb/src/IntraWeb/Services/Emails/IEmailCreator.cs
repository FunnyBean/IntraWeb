using MimeKit;

namespace IntraWeb.Services.Emails
{
    public interface IEmailCreator
    {

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        MimeMessage CreateEmail(string body);

    }
}
