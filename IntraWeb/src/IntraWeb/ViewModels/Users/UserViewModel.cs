using IntraWeb.Models.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntraWeb.ViewModels.Users
{
    /// <summary>
    /// User View Model for User.
    /// </summary>
    public class UserViewModel
    {
        [Required()]
        public int Id { get; set; }

        [Required()]
        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string Nickname { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Surname { get; set; }

        [JsonIgnore]
        public byte[] Photo { get; set; }

        public string PhotoBase64
        {
            get
            {
                if (this.Photo != null)
                {
                    return Convert.ToBase64String(this.Photo);
                }

                return null;
            }
            set
            {
                if (value != null)
                {
                    Photo = Convert.FromBase64String(value);
                }
            }
        }

        public ICollection<UserRoleViewModel> Roles { get; set; }
    }
}
