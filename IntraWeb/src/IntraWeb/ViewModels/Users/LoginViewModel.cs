using System.ComponentModel.DataAnnotations;

namespace IntraWeb.ViewModels.Users
{
    public class LoginViewModel
    {
        [Required()]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required()]
        [MaxLength(100)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
