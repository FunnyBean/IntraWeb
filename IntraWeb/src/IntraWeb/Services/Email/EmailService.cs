namespace IntraWeb.Services.Email
{
    /// <summary>
    /// Service for sending all email types.
    /// </summary>
    public class EmailService
        : IEmailService
    {

        #region Private members

        IEmailCreator _creator;
        IEmailSender _sender;

        #endregion


        #region Constructors

        public EmailService(IEmailCreator creator, IEmailSender sender)
        {
            _creator = creator;
            _sender = sender;
        }

        #endregion


        #region Common

        public string EmailFrom { get; set; } = Resources.Resources.EmailFrom;

        #endregion


        #region IEmailService

        /// <summary>
        /// Sends an email for resetting user's password.
        /// </summary>
        /// <param name="to">Email address of recepient.</param>
        /// <param name="resetLink">Link, where user can reset his password.</param>
        public void SendPasswordReset(string to, string resetLink)
        {
            var data = new PasswordResetData(resetLink);
            data.From = EmailFrom;
            data.To.Add(to);

            var msg = _creator.CreateEmail(data);
            _sender.SendEmail(msg);
        }

        #endregion

    }
}
