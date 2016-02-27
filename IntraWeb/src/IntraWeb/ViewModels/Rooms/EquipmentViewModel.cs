using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IntraWeb.ViewModels.Rooms
{
    /// <summary>
    /// Equipment View Model for Equipment.
    /// </summary>
    public class EquipmentViewModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [Required()]
        [MaxLength(100)]
        public string Description { get; set; }
    }
}
