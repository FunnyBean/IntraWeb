using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IntraWeb.ViewModels.Users
{
    public class LoginViewModel
    {
        [Required()]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required()]
        [MaxLength(100)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
