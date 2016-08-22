using Krosbook.ViewModels.Rooms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Citrix.GoToCoreLib.Api;
using Citrix.GoToMeeting.Api;
using System.Diagnostics;
using Krosbook.Models.Rooms;
using Citrix.GoToMeeting.Api.Model;

namespace Krosbook.Services.G2Meeting
{
    public class G2MService : IG2MService
    {
        private string accessToken;
        private IRoomRepository _roomRepository;

        public G2MService(IRoomRepository roomRepository)
        {
            string userName = "it@kros.sk";
            string userPassword = "21krosaci12";
            string consumerKey = "kHFkMQquDNQJMINHl07891ypZiqtidzT";
            var authApi = new AuthenticationApi();
            var response = authApi.directLogin(userName, userPassword, consumerKey, "password");
            this.accessToken = response.access_token;
            _roomRepository = roomRepository;
        }


        public Citrix.GoToMeeting.Api.Model.MeetingCreated createNewMeeting(RoomReservationViewModel roomResVM)
        {
            var meetingsApi = new MeetingsApi();
            var meeting = new Citrix.GoToMeeting.Api.Model.MeetingReqCreate();
            meeting.subject = roomResVM.name;
            meeting.starttime = roomResVM.dateTime.ToUniversalTime();
            meeting.endtime = roomResVM.dateTime.AddMinutes(roomResVM.length).ToUniversalTime();
            meeting.passwordrequired = false;
            meeting.conferencecallinfo = "Krosbook\n Rezervácia miestnosti: " + _roomRepository.GetItem(roomResVM.RoomId);
            meeting.meetingtype = MeetingType.scheduled;   
            meeting.conferencecallinfo = "Free";

            List<MeetingCreated> newMeeting = meetingsApi.createMeeting(this.accessToken, meeting);

            return newMeeting[0];
        }

        public bool canCreateMeeting(RoomReservationViewModel roomResVM)
        {
            bool canCreate = true;
            var meetings = this.getUpcomingMeetings();

            foreach (var meeting in meetings)
            {
                if (((roomResVM.dateTime >= meeting.startTime && roomResVM.dateTime < meeting.endTime) ||
                    (meeting.startTime >= roomResVM.dateTime && meeting.startTime < roomResVM.dateTime.AddMinutes(roomResVM.length))))
                {
                    canCreate = false;
                    break;
                }
            }
            return canCreate;
        }


        public void deleteMeeting(int meetingId)
        {
            var meetingsApi = new MeetingsApi();
            meetingsApi.deleteMeeting(this.accessToken, meetingId);
        }



        public List<UpcomingMeeting> getUpcomingMeetings()
        {
            var meetingsApi = new MeetingsApi();
            var meetings = meetingsApi.getUpcomingMeetings(accessToken);



            return meetings;
        }

        public MeetingCreated updateMeeting(RoomReservationViewModel reservationVm, int G2MeetingID)
        {
            var meetingsApi = new MeetingsApi();
            meetingsApi.deleteMeeting(this.accessToken, G2MeetingID);
         //   meetingsApi.
            return this.createNewMeeting(reservationVm);         
        }

        public string getMeetingUrl(int G2MeetingID)
        {
            var meetingsApi = new MeetingsApi();
            return meetingsApi.startMeeting(this.accessToken, long.Parse(G2MeetingID.ToString())).hostURL;
        }
    }
}
