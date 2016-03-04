using MimeKit;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace IntraWeb.Services.Email
{
    public class HtmlEmailCreator : IEmailCreator
    {

        IEmailFormatter _formatter;

        public HtmlEmailCreator(IEmailFormatter formatter)
        {
            _formatter = formatter;
        }


        public MimeMessage CreateEmail(string emailType, IDictionary<string, string> data)
        {
            var htmlBody = _formatter.FormatEmail(emailType, data);

            var msg = new MimeMessage();
            SetAddresses(msg, data);
            SetSubject(msg, htmlBody);

            var builder = new BodyBuilder();
            builder.HtmlBody = htmlBody;
            builder.TextBody = CreateTextBody(htmlBody);

            msg.Body = builder.ToMessageBody();

            return msg;
        }


        private void SetAddresses(MimeMessage msg, IDictionary<string, string> data)
        {
            SetAddress(msg.From, data, EmailDataKeys.From);
            SetAddress(msg.To, data, EmailDataKeys.To);
            SetAddress(msg.Cc, data, EmailDataKeys.Cc);
            SetAddress(msg.Bcc, data, EmailDataKeys.Bcc);
            SetAddress(msg.ReplyTo, data, EmailDataKeys.ReplyTo);
        }

        private void SetAddress(InternetAddressList addresses, IDictionary<string, string> data, string key)
        {
            string address = null;
            if (data.TryGetValue(key, out address))
            {
                addresses.Add(address);
            }
        }


        private Regex _reSubject = new Regex(@"<title>(.+?)</title>", RegexOptions.IgnoreCase);

        private void SetSubject(MimeMessage msg, string htmlBody)
        {
            var titleMatch = _reSubject.Match(htmlBody);
            if (titleMatch.Success)
            {
                msg.Subject = titleMatch.Groups[1].Value;
            }
        }


        private Regex _reHeader = new Regex(
            @"<(style|script|head).*?</\1>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private Regex _reTables = new Regex(@"<t[dh][ >]", RegexOptions.IgnoreCase);
        private Regex _reLinks = new Regex(
            @"<a [^>]*href=((""(?<href1>[^""]+)"")|('(?<href2>[^']+)'))[^>]*>(?<text>.*?)</a>", RegexOptions.IgnoreCase);
        private Regex _reNewLines = new Regex(@"[\r\n]+");
        private Regex _reParagraphs = new Regex(@"<(/?p|/?h\d|li|br|/tr)[ >/]", RegexOptions.IgnoreCase);
        private Regex _reHtmlTags = new Regex(@"<[^>]*>");
        private Regex _reLeadingWhitespace = new Regex(@"^[ \t]+", RegexOptions.Multiline);
        private Regex _reTrailingWhitespace = new Regex(@"[ \t]+\r?\n", RegexOptions.Multiline);
        private Regex _reWhitespace = new Regex(@"[ \t]+");

        private string CreateTextBody(string htmlBody)
        {
            var textBody = htmlBody;
            textBody = _reHeader.Replace(textBody, string.Empty);
            textBody = _reTables.Replace(textBody, " $0");
            textBody = _reLinks.Replace(textBody,
                                        (m) =>
                                        {
                                            var href = m.Groups["href1"].Value;
                                            if (href == "")
                                            {
                                                href = m.Groups["href2"].Value;
                                            }
                                            return $"{m.Groups["text"].Value} ({href})";
                                        });
            textBody = _reNewLines.Replace(textBody, " ");
            textBody = _reParagraphs.Replace(textBody, "\r\n$0");
            textBody = _reHtmlTags.Replace(textBody, string.Empty);
            textBody = System.Net.WebUtility.HtmlDecode(textBody);
            textBody = _reLeadingWhitespace.Replace(textBody, string.Empty);
            textBody = _reTrailingWhitespace.Replace(textBody, "\r\n");
            textBody = _reWhitespace.Replace(textBody, " ");

            return textBody.Trim();
        }

    }
}
