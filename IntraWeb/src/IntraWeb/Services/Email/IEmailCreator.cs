using MimeKit;
using System.Collections.Generic;

namespace IntraWeb.Services.Emails
{
    public interface IEmailCreator
    {

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        MimeMessage CreateEmail(string emailType, IDictionary<string, string> data);

    }
}
