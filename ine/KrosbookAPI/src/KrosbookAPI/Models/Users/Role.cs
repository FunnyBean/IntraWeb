using Krosbook.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Krosbook.Models.Users
{
    /// <summary>
    /// Model, which represent Role.
    /// </summary>
    public class Role : IModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [MaxLength(50)]
        [Required()]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public ICollection<UserRole> Users { get; set; }
    }
}
