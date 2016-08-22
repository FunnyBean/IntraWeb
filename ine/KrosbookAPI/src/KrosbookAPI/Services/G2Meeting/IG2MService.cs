using Krosbook.ViewModels.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Krosbook.Services.G2Meeting
{
    public interface IG2MService
    {
        List<Citrix.GoToMeeting.Api.Model.UpcomingMeeting> getUpcomingMeetings();

        Citrix.GoToMeeting.Api.Model.MeetingCreated createNewMeeting(RoomReservationViewModel roomResVM);

        Citrix.GoToMeeting.Api.Model.MeetingCreated updateMeeting(RoomReservationViewModel reservationVm, int G2MeetingID);

        string getMeetingUrl(int G2MeetingID);

        void deleteMeeting(int meetingId);

        bool canCreateMeeting(RoomReservationViewModel roomResVM);


    }
}
