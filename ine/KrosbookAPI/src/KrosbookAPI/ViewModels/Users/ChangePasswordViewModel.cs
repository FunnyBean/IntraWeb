using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Krosbook.ViewModels.Users
{
    public class ChangePasswordViewModel
    {
        public string oldPassword { get; set; }

        public string newPassword { get; set; }
    }
}
