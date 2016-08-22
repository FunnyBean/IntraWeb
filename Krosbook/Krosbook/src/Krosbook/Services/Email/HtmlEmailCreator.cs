using Krosbook.Services.Template;
using Microsoft.AspNetCore.Hosting;
using MimeKit;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Krosbook.Services.Email
{
    /// <summary>
    /// Implementation of <see cref="IEmailCreator" />, which creates HTML email messages. Resulting message has also
    /// plain text part, which is automatically created from HTML part.
    /// </summary>
    public class HtmlEmailCreator : IEmailCreator
    {

        private string _imagesFolder;
        private ITemplateFormatter _formatter;

        public HtmlEmailCreator(IHostingEnvironment env, ITemplateFormatter formatter)
        {
            var templateFolder = Path.Combine(env.WebRootPath, "templates", "email");
            _imagesFolder = Path.Combine(templateFolder, "images");
            _formatter = formatter;
        }


        /// <summary>
        /// Creates an email message with specified <paramref name="data" />.
        /// </summary>
        /// <param name="data">Data for the email.</param>
        /// <returns>Created message, which can be sent. Message is a HTML email. It also has plain text part, which is
        /// automatically created from HTML part.</returns>
        public MimeMessage CreateEmail(IEmailData data)
        {
            var htmlBody = _formatter.FormatTemplate(data.EmailType, data);

            var msg = new MimeMessage();
            SetAddresses(msg, data);
            SetSubject(msg, htmlBody);

            var builder = new BodyBuilder();
            builder.HtmlBody = htmlBody;
            builder.TextBody = CreateTextBody(htmlBody);
            AddLocalImages(builder, htmlBody);

            msg.Body = builder.ToMessageBody();

            return msg;
        }


        private void SetAddresses(MimeMessage msg, IEmailData data)
        {
            if (!string.IsNullOrWhiteSpace(data.From))
            {
                msg.From.Add(data.From);
            }
            SetAddress(msg.To, data.To);
            SetAddress(msg.Cc, data.Cc);
            SetAddress(msg.Bcc, data.Bcc);
            if (!string.IsNullOrWhiteSpace(data.ReplyTo))
            {
                msg.ReplyTo.Add(data.ReplyTo);
            }
        }

        private void SetAddress(InternetAddressList addressList, IEnumerable<string> emails)
        {
            foreach (var email in emails)
            {
                addressList.Add(email);
            }
        }


        private Regex _reSubject = new Regex(@"<title>(.+?)</title>", RegexOptions.IgnoreCase);

        private void SetSubject(MimeMessage msg, string htmlBody)
        {
            var titleMatch = _reSubject.Match(htmlBody);
            if (titleMatch.Success)
            {
                msg.Subject = System.Net.WebUtility.HtmlDecode(titleMatch.Groups[1].Value);
            }
        }


        private void AddLocalImages(BodyBuilder builder, string htmlBody)
        {
            foreach (var img in GetLocalImagesFromHtml(htmlBody))
            {
                var filePath = Path.Combine(_imagesFolder, img);
                if (File.Exists(filePath))
                {
                    builder.LinkedResources.Add(filePath);
                } else
                {
                    // Todo: Log invalid image in template.
                }
            }
        }


        private Regex _reImageSources = new Regex(
            @"<img [^>]*src=((""(?<src1>[^""]+)"")|('(?<src2>[^']+)'))", RegexOptions.IgnoreCase);
        private Regex _reImageProtocol = new Regex(@"^(https?://|/)", RegexOptions.IgnoreCase);

        private IEnumerable<string> GetLocalImagesFromHtml(string htmlBody)
        {
            var images = new HashSet<string>();

            foreach (Match m in _reImageSources.Matches(htmlBody))
            {
                var src = m.Groups["src1"].Value.Trim();
                if (src == "")
                {
                    src = m.Groups["src2"].Value.Trim();
                }
                if (!_reImageProtocol.IsMatch(src))
                {
                    images.Add(src);
                }
            }

            return images;
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
