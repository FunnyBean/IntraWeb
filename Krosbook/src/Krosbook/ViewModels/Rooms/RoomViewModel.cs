using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Krosbook.ViewModels.Rooms
{
    /// <summary>
    /// Room View Model for Room model
    /// </summary>
    public class RoomViewModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Required()]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required()]
        [MaxLength(50)]
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
        public ICollection<RoomEquipmentViewModel> Equipment { get; set; }



        public override string ToString()
        {
            return $"{this.Id} - {this.Name}";
        }
    }
}
