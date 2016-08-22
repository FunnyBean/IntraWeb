using Krosbook.Models.Rooms;
using Krosbook.Models.Users;
using Krosbook.Resources;
using Krosbook.Services.Template;
using Krosbook.ViewModels.Rooms;
using Microsoft.AspNetCore.Hosting;
using MimeKit;
using System;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace Krosbook.Services.Email
{
    /// <summary>
    /// Service for sending all email types.
    /// </summary>
    public class EmailService
        : IEmailService
    {

        #region Private members

        private IEmailCreator _creator;
        private IEmailSender _sender;
        private IUserRepository _userRepository;
        private IRoomRepository _roomRepository;
        private readonly IHostingEnvironment _appEnvironment;

        #endregion


        #region Constructors

        public EmailService(IEmailCreator creator, IEmailSender sender, IUserRepository userRepository, IHostingEnvironment appEnvironment, IRoomRepository roomRepository)
        {
            _creator = creator;
            _sender = sender;
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _appEnvironment = appEnvironment;
        }

        #endregion


        #region Common

        public string EmailFrom { get; set; } = Resources.Resources.EmailFrom;

        #endregion


        #region IEmailService

        /// <summary>
        /// Sends an email for resetting user's password.
        /// </summary>
        /// <param name="to">Email address of recepient.</param>
        /// <param name="resetLink">Link, where user can reset his password.</param>
        public void SendPasswordReset(string to, string resetLink)
        {
            var data = new PasswordResetData(resetLink);
            data.From = EmailFrom;
            data.To.Add(to);

            var msg = _creator.CreateEmail(data);
            _sender.SendEmail(msg);
        }


        public void CreateEmailCalendarEvent(RoomReservationViewModel rvm)
        {
            string schLocation = _roomRepository.GetItem(rvm.RoomId).Name;
            string schSubject = rvm.name;
            string schDescription = "Krosbook: Rezervácia miestnosti";
            string organizer = this._userRepository.GetItem(rvm.UserId).Email;

            System.DateTime schBeginDate = rvm.dateTime;
            System.DateTime schEndDate = rvm.dateTime.AddMinutes(rvm.length);

            String[] contents = { "BEGIN:VCALENDAR",
                              "PRODID:-//Flo Inc.//FloSoft//EN",
                              "BEGIN:VEVENT",
                              "DTSTART:" + schBeginDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
                              "DTEND:" + schEndDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
                              "LOCATION:" + schLocation,
                              "ORGANIZER:MAILTO:" + organizer,
            "DESCRIPTION;ENCODING=QUOTED-PRINTABLE:" + schDescription,
                              "SUMMARY:" + schSubject, "PRIORITY:3",
                         "END:VEVENT", "END:VCALENDAR" };

            Random rand = new Random(DateTime.Now.Millisecond);
            string filename = _appEnvironment.ContentRootPath + @"\Temp\CalendarEvent" + rand.Next() + ".ics";
            System.IO.File.WriteAllLines(filename, contents);
            SendRoomReservation(rvm.name, organizer, filename);
        }


        public void CreateEmailCalendarEvent(RoomReservationViewModel rvm, string joinUrlG2M)
        {
            string schLocation = _roomRepository.GetItem(rvm.RoomId).Name;
            string schSubject = rvm.name;
            string schDescription = "Krosbook: Rezervácia miestnosti";
            string organizer = this._userRepository.GetItem(rvm.UserId).Email;

            System.DateTime schBeginDate = rvm.dateTime;
            System.DateTime schEndDate = rvm.dateTime.AddMinutes(rvm.length);

            String[] contents = { "BEGIN:VCALENDAR",
                              "PRODID:-//Flo Inc.//FloSoft//EN",
                              "BEGIN:VEVENT",
                              "DTSTART:" + schBeginDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
                              "DTEND:" + schEndDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"),
                              "LOCATION:" + schLocation,
                              "ORGANIZER:MAILTO:" + organizer,
            "DESCRIPTION;ENCODING=QUOTED-PRINTABLE:" + schDescription,
                              "SUMMARY:" + schSubject, "PRIORITY:3",
                         "END:VEVENT", "END:VCALENDAR" };

            Random rand = new Random(DateTime.Now.Millisecond);
            string filename = _appEnvironment.ContentRootPath + @"\Temp\CalendarEvent" + rand.Next() + ".ics";
            System.IO.File.WriteAllLines(filename, contents);
            SendRoomReservation(rvm.name, organizer, filename, joinUrlG2M);
        }


        public void SendRoomReservation(string title, string emailTo, string filename)
        {
            var data = new RoomReservation(title);
            data.From = EmailFrom;
            data.To.Add(emailTo);
            var msg = _creator.CreateEmail(data);
            var builder = new BodyBuilder();
            builder.Attachments.Add(filename);
            msg.Body = builder.ToMessageBody();
            _sender.SendEmail(msg);
            File.Delete(filename);
        }

        public void SendRoomReservation(string title, string emailTo, string filename, string joinUrlG2M)
        {
            var data = new RoomReservation(title);
            data.From = EmailFrom;
            data.To.Add(emailTo);
            var msg = _creator.CreateEmail(data);
            var builder = new BodyBuilder();
            builder.HtmlBody = string.Format("<p>Na nasledujúcom odkaze: <a href="+joinUrlG2M+">" +joinUrlG2M+"</a> bol vytvorený meeting službou GoToMeeting</p>");
            builder.Attachments.Add(filename);
            msg.Body = builder.ToMessageBody();
            _sender.SendEmail(msg);
            File.Delete(filename);
        }

        public void SendG2M(RoomReservationViewModel rvm, string joinUrlG2M)
        {
            var data = new RoomReservation(rvm.name);
            data.From = EmailFrom;
            data.To.Add(this._userRepository.GetItem(rvm.UserId).Email);
            var msg = _creator.CreateEmail(data);
            var builder = new BodyBuilder();
            builder.HtmlBody = string.Format("<p>Na nasledujúcom odkaze: <a href=" + joinUrlG2M + ">" + joinUrlG2M + "</a> bol vytvorený meeting službou GoToMeeting</p>");     
            msg.Body = builder.ToMessageBody();
            _sender.SendEmail(msg);
        }


        #endregion



    }
}
