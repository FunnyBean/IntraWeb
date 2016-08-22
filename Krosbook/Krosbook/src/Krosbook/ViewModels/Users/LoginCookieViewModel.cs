using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Krosbook.ViewModels.Users
{
    public class LoginCookieViewModel
    {
        [Required()]
        public string Selector { get; set; }

        [Required()]
        public string Validator { get; set; }

        [Required()]
        public string NewValidator { get; set; }
    }
}
