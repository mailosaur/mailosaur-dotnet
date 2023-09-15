using System;
using Mailosaur.Models;
using Xunit;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Mailosaur.Test
{
    public class PreviewsFixture : IDisposable
    {
        public MailosaurClient client { get; set; }

        public string server { get; set; }

        public PreviewsFixture()
        {
            var baseUrl = Environment.GetEnvironmentVariable("MAILOSAUR_BASE_URL") ?? "https://mailosaur.com/";
            var apiKey = Environment.GetEnvironmentVariable("MAILOSAUR_API_KEY");
            server = Environment.GetEnvironmentVariable("MAILOSAUR_PREVIEWS_SERVER");

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new Exception("Missing necessary environment variables - refer to README.md");
            }

            client = new MailosaurClient(apiKey, baseUrl);
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }

    public class PreviewsTests : IClassFixture<PreviewsFixture>
    {
        PreviewsFixture fixture;

        public PreviewsTests(PreviewsFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void ListEmailClientsTest()
        {
            var result = this.fixture.client.Previews.ListEmailClients();
            Assert.True(result.Items.Count > 1);
        }

        [Fact]
        public void GeneratePreviewsTest()
        {
            // TODO When xUnit 3 is released, use Assert.SkipIf
            if (string.IsNullOrWhiteSpace(this.fixture.server))
            {
                return;
            }

            var randomString = Mailer.RandomString();
            var host = Environment.GetEnvironmentVariable("MAILOSAUR_SMTP_HOST") ?? "mailosaur.net";
            var testEmailAddress = $"{randomString}@{this.fixture.server}.{host}";

            Mailer.SendEmail(this.fixture.client, this.fixture.server, testEmailAddress);

            var email = this.fixture.client.Messages.Get(this.fixture.server, new SearchCriteria()
            {
                SentTo = testEmailAddress
            });

            PreviewRequest request = new PreviewRequest("OL2021");
            PreviewRequestOptions options = new PreviewRequestOptions(new List<PreviewRequest>() {
                request
            });

            PreviewListResult result = this.fixture.client.Messages.GeneratePreviews(email.Id, options);
            Assert.True(result.Items.Count > 0);

            byte[] bytes = this.fixture.client.Files.GetPreview(result.Items[0].Id);

            Assert.NotNull(bytes);
        }
    }
}
