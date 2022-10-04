using System;
using System.Linq;
using Mailosaur.Models;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mailosaur.Test
{
    public class FilesFixture : IDisposable
    {
        public MailosaurClient client { get; set; }

        public Message email { get; set; }

        public FilesFixture()
        {
            var baseUrl = Environment.GetEnvironmentVariable("MAILOSAUR_BASE_URL") ?? "https://mailosaur.com/";
            var apiKey = Environment.GetEnvironmentVariable("MAILOSAUR_API_KEY");
            var server = Environment.GetEnvironmentVariable("MAILOSAUR_SERVER");

            if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(server))
            {
                throw new Exception("Missing necessary environment variables - refer to README.md");
            }

            client = new MailosaurClient(apiKey, baseUrl);

            client.Messages.DeleteAll(server);

            var host = Environment.GetEnvironmentVariable("MAILOSAUR_SMTP_HOST") ?? "mailosaur.net";
            var testEmailAddress = $"wait_for_test@{server}.{host}";

            Mailer.SendEmail(client, server, testEmailAddress);

            email = client.Messages.Get(server, new SearchCriteria()
            {
                SentTo = testEmailAddress
            });
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }

    public class FilesTests : IClassFixture<FilesFixture>
    {
        private static readonly string s_Html = File.ReadAllText(Path.Combine("Resources", "testEmail.html"));
        private static readonly string s_Text = File.ReadAllText(Path.Combine("Resources", "testEmail.txt"));
        private static Random s_Random = new Random();

        FilesFixture fixture;

        public FilesTests(FilesFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void GetEmailTest()
        {
            byte[] bytes = this.fixture.client.Files.GetEmail(this.fixture.email.Id);

            Assert.NotNull(bytes);
            Assert.True(bytes.Length > 1);
            Assert.Contains(this.fixture.email.Subject, Encoding.UTF8.GetString(bytes));
        }

        [Fact]
        public void GetAttachmentTest()
        {
            Attachment attachment = this.fixture.email.Attachments[0];

            byte[] bytes = this.fixture.client.Files.GetAttachment(attachment.Id);

            Assert.NotNull(bytes);
            Assert.Equal(attachment.Length, bytes.Length);
        }
    }
}
