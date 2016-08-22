using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Krosbook.ViewModels.Reservation
{
    public class RoomReservationIntervalViewModel
    {
        [Required()]    
        public string from { get; set; }

        [Required()]
        public string to { get; set; }
    }
}
