namespace Krosbook.Models.Users
{
    /// <summary>
    /// Model, which represent join table between User and Role.
    /// </summary>
    public class UserRole
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the User.
        /// </summary>
        /// <value>
        /// The User.
        /// </value>
        /// <remarks>Navigation property.</remarks>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the role identifier.
        /// </summary>
        /// <value>
        /// The role identifier.
        /// </value>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the Role.
        /// </summary>
        /// <value>
        /// The Role.
        /// </value>
        /// <remarks>Navigation property.</remarks>
        public Role Role { get; set; }
    }
}
