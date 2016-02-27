using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IntraWeb.Models.Base;

namespace IntraWeb.Models.Rooms
{
    /// <summary>
    /// Model, which represent Room.
    /// </summary>
    public class Room: IModel
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
        /// Gets or sets the equipments.
        /// </summary>
        /// <value>
        /// The equipments.
        /// </value>
        public ICollection<RoomEquipment> Equipments { get; set; }
    }
}
