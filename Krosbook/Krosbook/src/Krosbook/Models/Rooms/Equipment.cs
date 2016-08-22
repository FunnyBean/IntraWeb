﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Krosbook.Models.Base;

namespace Krosbook.Models.Rooms
{
    /// <summary>
    /// Model, which represent Equipment.
    /// </summary>
    public class Equipment: IModel
    {

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [MaxLength(100)]
        [Required()]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the rooms.
        /// </summary>
        /// <value>
        /// The rooms.
        /// </value>
        public ICollection<RoomEquipment> Rooms { get; set; }
    }
}
