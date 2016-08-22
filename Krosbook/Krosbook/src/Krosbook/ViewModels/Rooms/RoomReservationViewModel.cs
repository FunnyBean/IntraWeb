using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Krosbook.ViewModels.Rooms
{
    public class RoomReservationViewModel
    {
        //[Required()]
        public int Id { get; set; }

        //[Required()]
        public int RoomId { get; set; }

        //[Required()]
        public int UserId { get; set; }

        ///[Required()]
        public DateTime dateTime { get; set; }

        public string date { get; set; }

        //[Required()]
        public int length { get; set; }

       // [Required()]
        public string name { get; set; }

        public bool emailInvitation { get; set; }

        public bool goToMeeting { get; set; }

        public int G2MeetingID { get; set; }

        public string FilterType { get; set; }
    }
}
