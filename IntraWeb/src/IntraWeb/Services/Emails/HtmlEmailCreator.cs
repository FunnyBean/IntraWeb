using System.Text.RegularExpressions;

using MimeKit;

namespace IntraWeb.Services.Emails
{
    public class HtmlEmailCreator : IEmailCreator
    {

        private Regex _reSubject = new Regex(@"<title>(.+?)</title>", RegexOptions.IgnoreCase);

        public MimeMessage CreateEmail(string htmlBody)
        {
            var msg = new MimeMessage();

            var titleMatch = _reSubject.Match(htmlBody);
            if (titleMatch.Success)
            {
                msg.Subject = titleMatch.Groups[1].Value;
            }

            return msg;
        }


        private Regex _reHeader = new Regex(
            @"<(style|script|head).*?</\1>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private Regex _reTables = new Regex(@"<t[dh][ >]", RegexOptions.IgnoreCase);
        private Regex _reLinks = new Regex(
            @"<a [^>]*href=""?(?<href>[^""]+)""?[^>]*>(?<text>.*?)</a>", RegexOptions.IgnoreCase);
        private Regex _reNewLines = new Regex(@"[\r\n]+");
        private Regex _reParagraphs = new Regex(@"<(/?p|/?h\d|li|br|/tr)[ >/]", RegexOptions.IgnoreCase);
        private Regex _reHtmlTags = new Regex(@"<[^>]*>");
        private Regex _reStartWhitespace = new Regex(@"^[ \t]+", RegexOptions.Multiline);
        private Regex _reWhitespace = new Regex(@"[ \t]+");

        public string CreateTextBody(string htmlBody)
        {
            var textBody = htmlBody;
            textBody = _reHeader.Replace(textBody, string.Empty);
            textBody = _reTables.Replace(textBody, " $0");
            textBody = _reLinks.Replace(textBody, "${text} (${href})");
            textBody = _reNewLines.Replace(textBody, " ");
            textBody = _reParagraphs.Replace(textBody, "\n$0");
            textBody = _reHtmlTags.Replace(textBody, string.Empty);
            textBody = System.Net.WebUtility.HtmlDecode(textBody);
            textBody = _reStartWhitespace.Replace(textBody, string.Empty);
            textBody = _reWhitespace.Replace(textBody, " ");

            return textBody;
        }

    }
}
