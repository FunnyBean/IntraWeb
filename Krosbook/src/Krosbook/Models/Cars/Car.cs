using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Krosbook.Models.Base;
using System.ComponentModel.DataAnnotations;
using Krosbook.Models.Reservation;

namespace Krosbook.Models.Cars
{
    /// <summary>
    /// Model, which represent Car.
    /// </summary>
    public class Car:IModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the plate.
        /// </summary>
        /// <value>
        /// The plate.
        /// </value>
        [MaxLength(20)]
        [Required()]
        public string Plate { get; set; }


        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [MaxLength(20)]
        [Required()]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Users.
        /// </summary>
        /// <value>
        /// The users.
        /// </value>
        public ICollection<CarReservation> Reservations { get; set; }


    }
}
