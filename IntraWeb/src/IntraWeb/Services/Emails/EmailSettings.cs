namespace IntraWeb.Services.Emails
{
    public class EmailSettings
    {

        public string Server { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
