using System;
using System.Linq;
using Mailosaur.Models;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Net.Mime;

namespace Mailosaur.Test
{
    public static class Mailer
    {
        private static readonly string s_Html = File.ReadAllText(Path.Combine("Resources", "testEmail.html"));
        private static readonly string s_Text = File.ReadAllText(Path.Combine("Resources", "testEmail.txt"));
        private static Random s_Random = new Random();

        public static void SendEmails(MailosaurClient client, string server, int quantity) {
            for (var i = 0; i < quantity; i++) 
                SendEmail(client, server);
        }

        public static void SendEmail(MailosaurClient client, string server, string sendToAddress = null)
        {
            var host = Environment.GetEnvironmentVariable("MAILOSAUR_SMTP_HOST") ?? "mailosaur.io";
			var port = Environment.GetEnvironmentVariable("MAILOSAUR_SMTP_PORT") ?? "25";

            var message = new MailMessage();

            var randomString = RandomString();

            message.Subject = randomString + " subject";

            message.From = new MailAddress(string.Format("{0} {1} <{2}>", randomString, randomString,
                client.Servers.GenerateEmailAddress(server)));

            var randomToAddress = sendToAddress ?? client.Servers.GenerateEmailAddress(server);

            message.To.Add(string.Format("{0} {1} <{2}>", randomString, randomString, randomToAddress));

            // Text body
			message.Body = s_Text.Replace("REPLACED_DURING_TEST", randomString);
			message.IsBodyHtml = false;
			message.BodyEncoding = Encoding.UTF8;

			// Html body
            var htmlString = s_Html.Replace("REPLACED_DURING_TEST", randomString);
			var htmlView = AlternateView.CreateAlternateViewFromString(htmlString,
                new ContentType(MediaTypeNames.Text.Html));
			htmlView.TransferEncoding = TransferEncoding.Base64;
			message.AlternateViews.Add(htmlView);

			var image = new LinkedResource(Path.Combine("Resources", "cat.png"));
			image.ContentId = "ii_1435fadb31d523f6";
            image.ContentType = new ContentType("image/png");
            htmlView.LinkedResources.Add(image);

            var attachment = new System.Net.Mail.Attachment(Path.Combine("Resources", "dog.png"));
            attachment.ContentType = new ContentType("image/png");
			message.Attachments.Add(attachment);

			var smtp = new SmtpClient();
			smtp.Host = host;
			smtp.Port = int.Parse(port);

			smtp.Send(message);
        }

        private static string RandomString() 
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
            .Select(s => s[s_Random.Next(s.Length)]).ToArray());
        }
    }
}
