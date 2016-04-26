using IntraWeb.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntraWeb.Models.Users
{
    /// <summary>
    /// Model, which represent Role.
    /// </summary>
    public class Role : IModel
    {

        #region Constants

        public const int NameMaxLength = 50;

        #endregion


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
        [Required]
        [MaxLength(NameMaxLength)]
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
