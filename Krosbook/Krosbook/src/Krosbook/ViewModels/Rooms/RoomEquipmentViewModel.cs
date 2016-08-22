using System.ComponentModel.DataAnnotations;

namespace Krosbook.ViewModels.Rooms
{
    /// <summary>
    /// View model for RoomEquipment model.
    /// </summary>
    public class RoomEquipmentViewModel
    {
        /// <summary>
        /// Gets or sets the equipment identifier.
        /// </summary>
        /// <value>
        /// The equipment identifier.
        /// </value>
        public int EquipmentId { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Amount.
        /// </summary>
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        public override string ToString()
        {
            return $"{this.EquipmentId} - {this.Description}";
        }
    }
}
