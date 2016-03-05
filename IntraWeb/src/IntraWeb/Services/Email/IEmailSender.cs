using MimeKit;

namespace IntraWeb.Services.Email
{
    public interface IEmailSender
    {

        void SendEmail(MimeMessage msg);

    }
}
