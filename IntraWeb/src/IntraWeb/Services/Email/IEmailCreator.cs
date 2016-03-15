using MimeKit;

namespace IntraWeb.Services.Email
{
    public interface IEmailCreator
    {

        MimeMessage CreateEmail(string emailType, IEmailData data);

    }
}
