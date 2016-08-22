using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Krosbook.ViewModels.Users
{
    public class LoginViewModel
    {
        [Required()]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required()]
        [MaxLength(100)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string Selector { get; set; }

        public string Validator { get; set; }
    }
}
