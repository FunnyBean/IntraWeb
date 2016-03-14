using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntraWeb.Services.Email
{
    public class PasswordResetData
        : BaseEmailData
    {

        #region Constructors

        public PasswordResetData(string emailType, string passwordResetLink)
        {
            this.EmailType = emailType;
            this.PasswordResetLink = passwordResetLink;
        }

        #endregion


        #region General

        [EmailDataKey("PasswordResetLink")]
        public string PasswordResetLink { get; set; }

        #endregion

    }
}
