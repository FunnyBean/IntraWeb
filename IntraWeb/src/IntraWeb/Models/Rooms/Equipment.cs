﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IntraWeb.Models.Base;

namespace IntraWeb.Models.Rooms
{
    /// <summary>
    /// Model, which represent Equipment.
    /// </summary>
    public class Equipment: IModel
    {

        #region Constants

        public const int DescriptionMaxLength = 100;

        #endregion


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
        [Required]
        [MaxLength(DescriptionMaxLength)]
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
