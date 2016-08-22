using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Krosbook.Models.Base;
using Krosbook.Models.Reservation;

namespace Krosbook.Models.Rooms
{
    /// <summary>
    /// Model, which represent Room.
    /// </summary>
    public class Room : IModel
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
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [MaxLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of room.
        /// </summary>
        /// <value>
        /// The room type.
        /// </value>
        [MaxLength(25)]
        [Required()]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the color of room.
        /// </summary>
        /// <value>
        /// The room color.
        /// </value>
        [MaxLength(7)]
        [Required()]
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the equipment.
        /// </summary>
        /// <value>
        /// The equipment.
        /// </value>
        public ICollection<RoomEquipment> Equipment { get; set; }

        /// <summary>
        /// Gets or sets the Users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public ICollection<RoomReservation> Reservations { get; set; }
    }

}

