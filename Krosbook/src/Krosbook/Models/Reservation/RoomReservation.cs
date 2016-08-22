using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Krosbook.Models.Rooms;
using Krosbook.Models.Users;
using Krosbook.Models.Base;

namespace Krosbook.Models.Reservation
{
    public class RoomReservation:IModel
    {

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
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        /// <remarks>Navigation property.</remarks>
        public User User { get; set; }

        /// <summary>
        /// Date and Time.
        /// </summary>       
        public DateTime dateTime { get; set; }

        ///length of reservation
        public int length { get; set; }


        public string name { get; set; }

        public int G2MeetingID { get; set; }
    }
}
