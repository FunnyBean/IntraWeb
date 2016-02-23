﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IntraWeb.ViewModels.Administration
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
        /// Gets or sets the equipments.
        /// </summary>
        /// <value>
        /// The equipments.
        /// </value>
        public ICollection<EquipmentViewModel> Equipments { get; set; }
    }
}
