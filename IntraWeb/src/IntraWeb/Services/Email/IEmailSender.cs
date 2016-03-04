using MimeKit;

namespace IntraWeb.Services.Emails
{
    public interface IEmailSender
    {

        void SendEmail(MimeMessage msg);

    }
}
