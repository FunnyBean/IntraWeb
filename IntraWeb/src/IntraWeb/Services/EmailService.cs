using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Threading.Tasks;

namespace IntraWeb.Services
{
    public class EmailService : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            builder.AddEnvironmentVariables();
            var config = builder.Build();
            string smtp = config["Email:SMTP:serverAddress"];
            int port = int.Parse(config["Email:SMTP:port"]);
            bool useSsl = bool.Parse(config["Email:SMTP:useSsl"]);
            string userName = config["Email:SMTP:userName"];
            string password = config["Email:SMTP:password"];



            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress("Joey Tribbiani", userName));
            msg.To.Add(new MailboxAddress("Majo Mikula", email));
            msg.Subject = subject;

            msg.Body = new TextPart("plain")
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                client.Connect(smtp, port, useSsl);

                if (useSsl)
                {
                    client.Authenticate(userName, password);
                }
                
                return client.SendAsync(msg);
            }
        }
    }
}
