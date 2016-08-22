using Krosbook.Services.Template;

namespace Krosbook.Services.Email
{
    /// <summary>
    /// Data for email message for resetting password.
    /// </summary>
    public class PasswordResetData: BaseEmailData
    {

        #region Constructors

        /// <summary>
        /// Initializes data with link for password reset <paramref name="passwordResetLink" />.
        /// </summary>
        /// <param name="passwordResetLink">Link where user can reset his password.</param>
        public PasswordResetData(string passwordResetLink): base("PasswordReset")
        {
            this.PasswordResetLink = passwordResetLink;
        }

        #endregion


        #region General

        /// <summary>
        /// Link where user can reset his password.
        /// </summary>
        [TemplateVariable("PasswordResetLink")]
        public string PasswordResetLink { get; }

        #endregion

    }
}
