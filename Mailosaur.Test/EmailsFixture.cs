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
    public class EmailsFixture
    {
        public MailosaurClient m_Client;
        public string s_ApiKey = Environment.GetEnvironmentVariable("MAILOSAUR_API_KEY");
        public string s_Server = Environment.GetEnvironmentVariable("MAILOSAUR_SERVER");
        public string s_BaseUrl = Environment.GetEnvironmentVariable("MAILOSAUR_BASE_URL");
        public readonly string s_DateIsoString = DateTime.Now.ToString("yyyy-MM-dd");
        private static readonly string s_Html = File.ReadAllText(Path.Combine("Resources", "testEmail.html"));
        private static readonly string s_Text = File.ReadAllText(Path.Combine("Resources", "testEmail.txt"));
        private static Random s_Random = new Random();
        public IList<Message> emails { get; set; }
        
        public EmailsFixture()
        {
            if (string.IsNullOrWhiteSpace(s_ApiKey) || string.IsNullOrWhiteSpace(s_Server)) {
                throw new Exception("Missing necessary environment variables - refer to README.md");
            }

            // To ensure reduce duplication and to ensure tests
            // are unaffected by emails being deleted mid-run,
            // this contructor performs tests of:
            //   - Emails.DeleteAll
            //   - Emails.List
            m_Client = new MailosaurClient(s_ApiKey, s_BaseUrl);

            m_Client.Messages.DeleteAll(s_Server);

			SendEmails(s_Server, 5);

            emails = m_Client.Messages.List(s_Server);
            Assert.Equal(5, emails.Count);
        }

        public void SendEmails(string server, int quantity) {
            for (var i = 0; i < quantity; i++) 
                SendEmail(server);

            // Wait to ensure email has arrived
			System.Threading.Thread.Sleep(2000);
        }

        public void SendEmail(string server, string sendToAddress = null)
        {
            var host = Environment.GetEnvironmentVariable("MAILOSAUR_SMTP_HOST") ?? "mailosaur.io";
			var port = Environment.GetEnvironmentVariable("MAILOSAUR_SMTP_PORT") ?? "25";

            var message = new MailMessage();

            var randomString = RandomString();

            message.Subject = randomString + " subject";

            message.From = new MailAddress(string.Format("{0} {1} <{2}>", randomString, randomString,
                m_Client.Servers.GenerateEmailAddress(server)));

            var randomToAddress = sendToAddress ?? m_Client.Servers.GenerateEmailAddress(server);

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

			var client = new SmtpClient();
			client.Host = host;
			client.Port = int.Parse(port);

			client.Send(message);
        }

        private string RandomString() 
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
            .Select(s => s[s_Random.Next(s.Length)]).ToArray());
        }
    }

    [CollectionDefinition("Mailosaur Client")]
    public class MailosaurCollection : ICollectionFixture<EmailsFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
