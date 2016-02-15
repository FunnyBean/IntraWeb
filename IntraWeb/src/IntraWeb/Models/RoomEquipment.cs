using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntraWeb.Models
{
    /// <summary>
    /// Model, which represent join table between Room and equipment.
    /// </summary>
    /// <remarks>
    /// Many-to-many relationships without an entity class to represent the join table are not yet supported. 
    /// <see href="http://ef.readthedocs.org/en/latest/modeling/relationships.html#many-to-many"/>
    /// </remarks>
    public class RoomEquipment
    {

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

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
    }
}
