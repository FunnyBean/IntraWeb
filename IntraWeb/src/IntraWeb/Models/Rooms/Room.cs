using IntraWeb.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntraWeb.Models.Rooms
{
    /// <summary>
    /// Model, which represent Room.
    /// </summary>
    public class Room : IModel
    {

        #region Constants

        public const int NameMaxLength = 100;
        public const int TypeMaxLength = 100;
        public const int DescriptionMaxLength = 255;

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
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required()]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of room.
        /// </summary>
        /// <value>
        /// The room type.
        /// </value>
        [Required]
        [MaxLength(TypeMaxLength)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the equipment.
        /// </summary>
        /// <value>
        /// The equipment.
        /// </value>
        public ICollection<RoomEquipment> Equipment { get; set; }

    }
}
