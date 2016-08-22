using System.ComponentModel.DataAnnotations;

namespace Krosbook.Models.Rooms
{
    /// <summary>
    /// Model, which represent join table between Room and equipment.
    /// </summary>
    public class RoomEquipment
    {
        /// <summary>
        /// Gets or sets the room identifier.
        /// </summary>
        /// <value>
        /// The room identifier.
        /// </value>
        public int RoomId { get; set; }

        /// <summary>
        /// Gets or sets the room.
        /// </summary>
        /// <value>
        /// The room.
        /// </value>
        /// <remarks>Navigation property.</remarks>
        public Room Room { get; set; }


        /// <summary>
        /// Gets or sets the equipment identifier.
        /// </summary>
        /// <value>
        /// The equipment identifier.
        /// </value>
        public int EquipmentId { get; set; }

        /// <summary>
        /// Gets or sets the equipment.
        /// </summary>
        /// <value>
        /// The equipment.
        /// </value>
        /// <remarks>Navigation property.</remarks>
        public Equipment Equipment { get; set; }

        /// <summary>
        /// Amount.
        /// </summary>
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }
    }
}
