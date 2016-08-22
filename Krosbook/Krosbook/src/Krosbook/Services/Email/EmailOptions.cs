namespace Krosbook.Services.Email
{
    /// <summary>
    /// Connection options for email services.
    /// </summary>
    public class EmailOptions
    {

        /// <summary>
        /// Email server's address..
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Email server's port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Indicates, if the server uses secure connection.
        /// </summary>
        public bool UseSsl { get; set; }

        /// <summary>
        /// Username for connecting to server.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password for connecting to server.
        /// </summary>
        public string Password { get; set; }

    }
}
