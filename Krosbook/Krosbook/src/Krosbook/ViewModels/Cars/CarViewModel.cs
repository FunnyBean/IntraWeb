using Krosbook.Models.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Krosbook.ViewModels.Cars
{
    /// <summary>
    /// Car View Model for User.
    /// </summary>
    public class CarViewModel
    {
        //  [Required()]
        public int Id { get; set; }

        [Required()]
        [MaxLength(100)]
        public string Plate { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }


        public override string ToString()
        {
            return $"{this.Id} - {this.Name}";
        }
    }
}
