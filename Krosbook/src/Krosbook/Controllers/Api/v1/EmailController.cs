using Microsoft.AspNetCore.Mvc;
using Krosbook.Services.Email;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Krosbook.Services.G2Meeting;

namespace Krosbook.Controllers.Api.v1
{

    [Route("api/email")]
    [EnableCors("AllowAll")]
    public class EmailController : BaseController
    {

        #region Private members

        private IEmailService _emailService;
        private IEmailCreator _creator;
        private IEmailSender _sender;
        private readonly IHostingEnvironment _appEnvironment;
        private IG2MService _G2MService;

        #endregion


        public EmailController(IEmailService emailService, IEmailCreator creator, IEmailSender sender, IHostingEnvironment appEnvironment, IG2MService G2MService)
        {
            _emailService = emailService;
            _creator = creator;
            _sender = sender;
            _appEnvironment = appEnvironment;
            _G2MService = G2MService;
        }


        [HttpGet]
        [Route("send/")]
        public  void Send()
        {
         //   CreateCalendarEvent();         
      
           

            /*  var data = new BaseEmailData(emailType);
              data.From = Resources.Resources.EmailFrom;
              data.To.Add(to);

              var msg = _creator.CreateEmail(data);
         }     _sender.SendEmail(msg);*/

        }

        [HttpGet]
        [Route("PasswordReset")]
        public void PasswordReset(string to)
        {
            var data = new PasswordResetData(@"http://example.com");
            data.From = Resources.Resources.EmailFrom;
            data.To.Add(to);
            var msg = _creator.CreateEmail(data);
            _sender.SendEmail(msg);
        }

        [HttpGet]
        [Route("example")]
        public void PasswordReset2(string to)
        {
            if (_G2MService.canCreateMeeting(new ViewModels.Rooms.RoomReservationViewModel()))
            {
                _G2MService.createNewMeeting(new ViewModels.Rooms.RoomReservationViewModel());
           }

            _G2MService.getUpcomingMeetings();

            


            //  _emailService.SendPasswordReset(to, @"http://example.com");
        }





    }
}
