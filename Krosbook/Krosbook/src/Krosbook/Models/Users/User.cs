using System.Collections.Generic;
using Krosbook.Models.Base;
using System;
using System.ComponentModel.DataAnnotations;
using Krosbook.Models.Reservation;

namespace Krosbook.Models.Users
{
    /// <summary>
    /// Model, which represent User.
    /// </summary>
    public class User : IModel
    {

        #region Static helpers
        public static int BCryptWorkFactor { get; set; } = 10;

        #endregion


        #region Constants

        public const int EmailMaxLength = 100;
        public const int UserNameMaxLength = 100;
        public const int NameMaxLength = 100;
        public const int SurnameMaxLength = 100;
        public const int PasswordHashMaxLength = 100;

        #endregion


        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [Required()]
        [MaxLength(EmailMaxLength)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The user name.
        /// </value>
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The user surname.
        /// </value>
        [Required]
        [MaxLength(SurnameMaxLength)]
        public string Surname { get; set; }

        public string Password { set { PasswordHash = BCrypt.Net.BCrypt.HashPassword(value, BCryptWorkFactor); } }

        /// <summary>
        /// Gets or sets the hashed password.
        /// </summary>
        /// <value>
        /// The hashed password.
        /// </value>
        [Required]
        [MaxLength(PasswordHashMaxLength)]
        public string PasswordHash { get; set; }


        [Required]
        public bool IsLocked { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public byte[] Photo { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public ICollection<UserRole> Roles { get; set; } = new List<UserRole>();




        /// <summary>
        /// Gets or sets the rooms.
        /// </summary>
        /// <value>
        /// The cars.
        /// </value>
        public ICollection<RoomReservation> Rooms { get; set; }
        /// <summary>
        /// Gets or sets the cars.
        /// </summary>
        /// <value>
        /// The cars.
        /// </value>
        public ICollection<CarReservation> Cars { get; set; } = new List<CarReservation>();
        public static object Claims { get; internal set; }

        public ICollection<RememberMe> rememberMe { get; set; }
    }
}
