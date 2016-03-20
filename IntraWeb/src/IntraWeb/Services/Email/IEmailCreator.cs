using MimeKit;

namespace IntraWeb.Services.Email
{
    public interface IEmailCreator
    {

        MimeMessage CreateEmail(IEmailData data);

    }
}
