namespace IntraWeb.Services.Email
{
    public interface IEmailService
    {
        void SendPasswordReset(string to, string resetLink);
    }
}
