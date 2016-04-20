using System.Collections.Generic;
using IntraWeb.Models.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace IntraWeb.Models.Users
{
    /// <summary>
    /// Model, which represent User.
    /// </summary>
    public class User : IModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [MaxLength(100)]
        [Required()]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the nickname.
        /// </summary>
        /// <value>
        /// The nickname.
        /// </value>
        [MaxLength(100)]
        public string Nickname { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The user name.
        /// </value>
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The user surname.
        /// </value>
        [MaxLength(100)]
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the hashed password.
        /// </summary>
        /// <value>
        /// The hashed password.
        /// </value>
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public bool IsLocked { get; set; }
        public DateTime DateCreated { get; set; }
        public byte[] Photo { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public ICollection<UserRole> Roles { get; set; } = new List<UserRole>();
    }
}
