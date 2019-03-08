using System;
using System.Linq;
using Mailosaur.Models;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mailosaur.Test
{
    public class EmailsFixture : IDisposable
    {
        public MailosaurClient client { get; set; }

        public string server { get; set; }
        
        public IList<MessageSummary> emails { get; set; }

        public EmailsFixture()
        {
            var baseUrl = Environment.GetEnvironmentVariable("MAILOSAUR_BASE_URL") ?? "https://mailosaur.com/";
            var apiKey  = Environment.GetEnvironmentVariable("MAILOSAUR_API_KEY");
            server = Environment.GetEnvironmentVariable("MAILOSAUR_SERVER");

            if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(server)) {
                throw new Exception("Missing necessary environment variables - refer to README.md");
            }

            client = new MailosaurClient(apiKey, baseUrl);

            client.Messages.DeleteAll(server);

            Mailer.SendEmails(client, server, 5);

            emails = client.Messages.List(server).Items;
        }

        public void Dispose() { }
    }

    public class EmailsTests : IClassFixture<EmailsFixture>
    {
        EmailsFixture fixture;
        
        public EmailsTests(EmailsFixture fixture)
        {
            this.fixture = fixture;
            
            foreach (var email in this.fixture.emails)
                ValidateEmailSummary(email);
        }

        [Fact]
        public void GetTest()
        {
            var emailToRetrieve = this.fixture.emails[0];
            Message email = this.fixture.client.Messages.Get(emailToRetrieve.Id);
            ValidateEmail(email);
            ValidateHeaders(email);
        }

        [Fact]
        public void GetNotFoundTest()
        {
            // Should throw if email is not found
            Assert.Throws<MailosaurException>(delegate {
                this.fixture.client.Messages.Get("");
            });
        }

        [Fact]
        public void WaitForTest()
        {
            var host = Environment.GetEnvironmentVariable("MAILOSAUR_SMTP_HOST") ?? "mailosaur.io";
            var testEmailAddress = string.Format("wait_for_test.{0}@{1}", fixture.server, host);
            
            Mailer.SendEmail(fixture.client, fixture.server, testEmailAddress);

            Message email = this.fixture.client.Messages
                .WaitFor(this.fixture.server, new SearchCriteria() {
                SentTo = testEmailAddress
            });

            ValidateEmail(email);
        }

        [Fact]
        public void SearchNoCriteriaErrorTest()
        {
            Assert.Throws<MailosaurException>(delegate {
                this.fixture.client.Messages
                    .Search(this.fixture.server, new SearchCriteria());
            });
        }

        [Fact]
        public void SearchBySentToTest()
        {
            var targetEmail = this.fixture.emails[1];
            
            var results = this.fixture.client.Messages
                .Search(this.fixture.server, new SearchCriteria() {
                SentTo = targetEmail.To[0].Email
            }).Items;

            Assert.Equal(1, results.Count);
            Assert.Equal(targetEmail.To[0].Email, results[0].To[0].Email);
            Assert.Equal(targetEmail.Subject, results[0].Subject);
        }

        [Fact]
        public void SearchBySentToInvalidEmailTest()
        {
            var criteria = new SearchCriteria() {
                SentTo = ".not_an_email_address"
            };

            Assert.Throws<MailosaurException>(delegate {
                this.fixture.client.Messages
                    .Search(this.fixture.server, criteria);
            });
        }

        [Fact]
        public void SearchByBodyTest()
        {
            var targetEmail = this.fixture.emails[1];
            var uniqueString = targetEmail.Subject.Substring(0, 10);
            
            var results = this.fixture.client.Messages.Search(this.fixture.server, new SearchCriteria() {
                Body = uniqueString + " html"
            }).Items;

            Assert.Equal(1, results.Count);
            Assert.Equal(targetEmail.To[0].Email, results[0].To[0].Email);
            Assert.Equal(targetEmail.Subject, results[0].Subject);
        }

        [Fact]
        public void SearchBySubjectTest()
        {
            var targetEmail = this.fixture.emails[1];
            var uniqueString = targetEmail.Subject.Substring(0, 10);

            var results = this.fixture.client.Messages.Search(this.fixture.server, new SearchCriteria() {
                Subject = uniqueString
            }).Items;

            Assert.Equal(1, results.Count);
            Assert.Equal(targetEmail.To[0].Email, results[0].To[0].Email);
            Assert.Equal(targetEmail.Subject, results[0].Subject);
        }

        [Fact]
        public void SpamAnalysisTest()
        {
            var targetId = this.fixture.emails[0].Id;
            SpamAnalysisResult result = this.fixture.client.Analysis.Spam(targetId);
            foreach (SpamAssassinRule rule in result.SpamFilterResults.SpamAssassin)
            {
                Assert.NotEmpty(rule.Rule);
                Assert.NotEmpty(rule.Description);
            }
        }

        [Fact]
        public void DeleteTest()
        {
            var targetEmailId = this.fixture.emails[4].Id;
            var self = this;

            this.fixture.client.Messages.Delete(targetEmailId);
        
            // Attempting to delete again should fail
            Assert.Throws<MailosaurException>(delegate {
                self.fixture.client.Messages.Delete(targetEmailId);
            });
        }

        private void ValidateEmail(Message email)
        {
            ValidateMetadata(email);
            ValidateAttachmentMetadata(email);
            ValidateHtml(email);
            ValidateText(email);
        }

        private void ValidateEmailSummary(MessageSummary email)
        {
            ValidateMetadata(email);
            Assert.Equal(2, email.Attachments);
        }

        private void ValidateHtml(Message email)
        {
            // Html.Body
            Assert.StartsWith("<div dir=\"ltr\">", email.Html.Body);

            // Html.Links
            Assert.Equal(3, email.Html.Links.Count);
            Assert.Equal("https://mailosaur.com/", email.Html.Links[0].Href);
            Assert.Equal("mailosaur", email.Html.Links[0].Text);
            Assert.Equal("https://mailosaur.com/", email.Html.Links[1].Href);
            Assert.Null(email.Html.Links[1].Text);
            Assert.Equal("http://invalid/", email.Html.Links[2].Href);
            Assert.Equal("invalid", email.Html.Links[2].Text);

            // Html.Images
            Assert.True(email.Html.Images[1].Src.StartsWith("cid:"));
            Assert.Equal("Inline image 1", email.Html.Images[1].Alt);
        }

        private void ValidateText(Message email)
        {
            // Text.Body
            Assert.StartsWith("this is a test", email.Text.Body);
            
            // Text.Links
            Assert.Equal(2, email.Text.Links.Count);
            Assert.Equal("https://mailosaur.com/", email.Text.Links[0].Href);
            Assert.Equal( email.Text.Links[0].Href, email.Text.Links[0].Text);
            Assert.Equal("https://mailosaur.com/", email.Text.Links[1].Href);
            Assert.Equal( email.Text.Links[1].Href, email.Text.Links[1].Text);
        }

        private void ValidateHeaders(Message email)
        {
            var expectedFromHeader = string.Format("\"{0}\" <{1}>", email.From[0].Name, email.From[0].Email);
            var expectedToHeader = string.Format("\"{0}\" <{1}>", email.To[0].Name, email.To[0].Email);

            // Fallback casing is used, as header casing is determined by sending server
            // Assert.Equal(expectedFromHeader, actual.Headers.ContainsKey("From") ?
            //     actual.Headers["From"].ToString() : actual.Headers["from"].ToString());
            
            // Assert.Equal(expectedToHeader, actual.Headers.ContainsKey("To") ?
            //     actual.Headers["To"].ToString() : actual.Headers["to"].ToString());
            
            // Assert.Equal(expected.Subject, actual.Headers.ContainsKey("Subject") ?
            //     actual.Headers["Subject"].ToString() : actual.Headers["subject"].ToString());
        }

        private void ValidateMetadata(Message email) {
            ValidateMetadata(new MessageSummary() {
                From = email.From,
                To = email.To,
                Cc = email.Cc,
                Bcc = email.Bcc,
                Subject = email.Subject,
                Received = email.Received
            });
        }

        private void ValidateMetadata(MessageSummary email)
        {
            Assert.Equal(1, email.From.Count);
            Assert.Equal(1, email.To.Count);
            Assert.NotEmpty(email.From[0].Email);
            Assert.NotEmpty(email.From[0].Name);
            Assert.NotEmpty(email.To[0].Email);
            Assert.NotEmpty(email.To[0].Name);
            Assert.NotEmpty(email.Subject);

            Assert.Equal(DateTime.Now.ToString("yyyy-MM-dd"), email.Received.Value.ToString("yyyy-MM-dd"));
        }

        private void ValidateAttachmentMetadata(Message email)
        {
            Assert.Equal(2, email.Attachments.Count);

            var file1 = email.Attachments[0];
            Assert.NotNull(file1.Id);
            Assert.Equal(82138, file1.Length);
            Assert.NotNull(file1.Url);
            Assert.Equal("ii_1435fadb31d523f6", file1.FileName);
            Assert.Equal("image/png", file1.ContentType);

            var file2 = email.Attachments[1];
            Assert.NotNull(file2.Id);
            Assert.Equal(212080, file2.Length);
            Assert.NotNull(file2.Url);
            Assert.Equal("Resources/dog.png", file2.FileName);
            Assert.Equal("image/png", file2.ContentType);
        }
    }
}