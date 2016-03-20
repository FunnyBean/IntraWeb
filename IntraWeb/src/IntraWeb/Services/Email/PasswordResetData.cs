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

        public PasswordResetData(string passwordResetLink)
            : base("PasswordReset")
        {
            this.PasswordResetLink = passwordResetLink;
        }

        #endregion


        #region General

        [TemplateVariable("PasswordResetLink")]
        public string PasswordResetLink { get; set; }

        #endregion

    }
}
