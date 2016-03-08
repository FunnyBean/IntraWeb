using System.Collections.Generic;
using IntraWeb.Models.Base;
using System;

namespace IntraWeb.Models.Users
{
    /// <summary>
    /// Model, which represent User.
    /// </summary>
    public class User : IModel
    {
        public User()
        {
            UserRoles = new List<UserRole>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public bool IsLocked { get; set; }
        public DateTime DateCreated { get; set; }
        public byte[] Photo { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
