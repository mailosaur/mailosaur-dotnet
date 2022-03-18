using System;
using System.Linq;
using Mailosaur.Models;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace Mailosaur.Test
{
    public class EmailsFixture : IDisposable
    {
        public MailosaurClient client { get; set; }

        public string server { get; set; }

        public string verifiedDomain { get; set; }

        public IList<MessageSummary> emails { get; set; }

        public EmailsFixture()
        {
            var baseUrl = Environment.GetEnvironmentVariable("MAILOSAUR_BASE_URL") ?? "https://mailosaur.com/";
            var apiKey = Environment.GetEnvironmentVariable("MAILOSAUR_API_KEY");
            server = Environment.GetEnvironmentVariable("MAILOSAUR_SERVER");
            verifiedDomain = Environment.GetEnvironmentVariable("MAILOSAUR_VERIFIED_DOMAIN");

            if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(server))
            {
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
        public void ListReceivedAfterTest()
        {
            var pastDate = DateTime.Now.AddMinutes(-10);
            var pastEmails = this.fixture.client.Messages
                .List(this.fixture.server, receivedAfter: pastDate).Items;

            Assert.True(pastEmails.Count > 0);

            var futureEmails = this.fixture.client.Messages
                .List(this.fixture.server, receivedAfter: DateTime.Now).Items;

            Assert.Equal(0, futureEmails.Count);
        }

        [Fact]
        public void GetTest()
        {
            var host = Environment.GetEnvironmentVariable("MAILOSAUR_SMTP_HOST") ?? "mailosaur.net";
            var testEmailAddress = $"wait_for_test@{fixture.server}.{host}";

            Mailer.SendEmail(fixture.client, fixture.server, testEmailAddress);

            Message email = this.fixture.client.Messages
                .Get(this.fixture.server, new SearchCriteria()
                {
                    SentTo = testEmailAddress
                });

            ValidateEmail(email);
        }

        [Fact]
        public void GetByIdTest()
        {
            var emailToRetrieve = this.fixture.emails[0];
            Message email = this.fixture.client.Messages.GetById(emailToRetrieve.Id);
            ValidateEmail(email);
            ValidateHeaders(email);
        }

        [Fact]
        public void GetByIdNotFoundTest()
        {
            // Should throw if email is not found
            Assert.Throws<MailosaurException>(delegate
            {
                this.fixture.client.Messages.GetById("efe907e9-74ed-4113-a3e0-a3d41d914765");
            });
        }

        [Fact]
        public void SearchNoCriteriaErrorTest()
        {
            Assert.Throws<MailosaurException>(delegate
            {
                this.fixture.client.Messages
                    .Search(this.fixture.server, new SearchCriteria());
            });
        }

        [Fact]
        public void SearchTimeoutErrorSuppressedTest()
        {
            var results = this.fixture.client.Messages
                .Search(this.fixture.server, new SearchCriteria()
                {
                    SentFrom = "neverfound@example.com"
                }, timeout: 1, errorOnTimeout: false).Items;

            Assert.Equal(0, results.Count);
        }

        [Fact]
        public void SearchBySentFromTest()
        {
            var targetEmail = this.fixture.emails[1];

            var results = this.fixture.client.Messages
                .Search(this.fixture.server, new SearchCriteria()
                {
                    SentFrom = targetEmail.From[0].Email
                }).Items;

            Assert.Equal(1, results.Count);
            Assert.Equal(targetEmail.From[0].Email, results[0].From[0].Email);
            Assert.Equal(targetEmail.Subject, results[0].Subject);
        }

        [Fact]
        public void SearchBySentToTest()
        {
            var targetEmail = this.fixture.emails[1];

            var results = this.fixture.client.Messages
                .Search(this.fixture.server, new SearchCriteria()
                {
                    SentTo = targetEmail.To[0].Email
                }).Items;

            Assert.Equal(1, results.Count);
            Assert.Equal(targetEmail.To[0].Email, results[0].To[0].Email);
            Assert.Equal(targetEmail.Subject, results[0].Subject);
        }

        [Fact]
        public void SearchByBodyTest()
        {
            var targetEmail = this.fixture.emails[1];
            var uniqueString = targetEmail.Subject.Substring(0, 10);

            var results = this.fixture.client.Messages.Search(this.fixture.server, new SearchCriteria()
            {
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

            var results = this.fixture.client.Messages.Search(this.fixture.server, new SearchCriteria()
            {
                Subject = uniqueString
            }).Items;

            Assert.Equal(1, results.Count);
            Assert.Equal(targetEmail.To[0].Email, results[0].To[0].Email);
            Assert.Equal(targetEmail.Subject, results[0].Subject);
        }

        [Fact]
        public void SearchWithMatchAll()
        {
            var targetEmail = this.fixture.emails[1];
            var uniqueString = targetEmail.Subject.Substring(0, 10);

            var results = this.fixture.client.Messages.Search(this.fixture.server, new SearchCriteria()
            {
                Subject = uniqueString,
                Body = "this is a link",
                Match = SearchMatchOperator.ALL
            }).Items;

            Assert.Equal(1, results.Count);
        }

        [Fact]
        public void SearchWithMatchAny()
        {
            var targetEmail = this.fixture.emails[1];
            var uniqueString = targetEmail.Subject.Substring(0, 10);

            var results = this.fixture.client.Messages.Search(this.fixture.server, new SearchCriteria()
            {
                Subject = uniqueString,
                Body = "this is a link",
                Match = SearchMatchOperator.ANY
            }).Items;

            Assert.Equal(5, results.Count);
        }

        [Fact]
        public void SearchWithSpecialCharactersTest()
        {
            var results = this.fixture.client.Messages.Search(this.fixture.server, new SearchCriteria()
            {
                Subject = "Search with ellipsis … and emoji 👨🏿‍🚒"
            }).Items;

            Assert.Equal(0, results.Count);
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
            Assert.Throws<MailosaurException>(delegate
            {
                self.fixture.client.Messages.Delete(targetEmailId);
            });
        }

        [Fact]
        public void CreateSendTextTest()
        {
            // TODO When xUnit 3 is released, use Assert.SkipIf
            if (string.IsNullOrWhiteSpace(this.fixture.verifiedDomain))
            {
                return;
            }

            var subject = "New message";

            var message = this.fixture.client.Messages.Create(this.fixture.server, new MessageCreateOptions()
            {
                To = $"anything@{fixture.verifiedDomain}",
                Send = true,
                Subject = subject,
                Text = "This is a new email"
            });

            Assert.NotEmpty(message.Id);
            Assert.Equal(subject, message.Subject);
        }

        [Fact]
        public void CreateSendHtmlTest()
        {
            // TODO When xUnit 3 is released, use Assert.SkipIf
            if (string.IsNullOrWhiteSpace(this.fixture.verifiedDomain))
            {
                return;
            }

            var subject = "New HTML message";

            var message = this.fixture.client.Messages.Create(this.fixture.server, new MessageCreateOptions()
            {
                To = $"anything@{fixture.verifiedDomain}",
                Send = true,
                Subject = subject,
                Html = "<p>This is a new email.</p>"
            });

            Assert.NotEmpty(message.Id);
            Assert.Equal(subject, message.Subject);
        }

        [Fact]
        public void CreateSendWithAttachment()
        {
            // TODO When xUnit 3 is released, use Assert.SkipIf
            if (string.IsNullOrWhiteSpace(this.fixture.verifiedDomain))
            {
                return;
            }

            var subject = "New message with attachment";

            var data = File.ReadAllBytes(Path.Combine("Resources", "cat.png"));
            var attachment = new Attachment()
            {
                FileName = "cat.png",
                Content = Convert.ToBase64String(data),
                ContentType = "image/png"
            };

            var message = this.fixture.client.Messages.Create(this.fixture.server, new MessageCreateOptions()
            {
                To = $"anything@{fixture.verifiedDomain}",
                Send = true,
                Subject = subject,
                Html = "<p>This is a new email.</p>",
                Attachments = new List<Attachment>() { attachment }
            });

            Assert.Equal(1, message.Attachments.Count);
            var file1 = message.Attachments[0];
            Assert.NotNull(file1.Id);
            Assert.Equal(82138, file1.Length);
            Assert.NotNull(file1.Url);
            Assert.Equal("cat.png", file1.FileName);
            Assert.Equal("image/png", file1.ContentType);
        }

        [Fact]
        public void ForwardTextTest()
        {
            // TODO When xUnit 3 is released, use Assert.SkipIf
            if (string.IsNullOrWhiteSpace(this.fixture.verifiedDomain))
            {
                return;
            }

            var body = "Forwarded message";
            var targetEmail = this.fixture.emails[0];

            var message = this.fixture.client.Messages.Forward(targetEmail.Id, new MessageForwardOptions()
            {
                To = $"anything@{fixture.verifiedDomain}",
                Text = body
            });

            Assert.NotEmpty(message.Id);
            Assert.Contains(body, message.Text.Body);
        }

        [Fact]
        public void ForwardHtmlTest()
        {
            // TODO When xUnit 3 is released, use Assert.SkipIf
            if (string.IsNullOrWhiteSpace(this.fixture.verifiedDomain))
            {
                return;
            }

            var body = "<p>Forwarded <strong>HTML</strong> message.</p>";
            var targetEmail = this.fixture.emails[0];

            var message = this.fixture.client.Messages.Forward(targetEmail.Id, new MessageForwardOptions()
            {
                To = $"anything@{fixture.verifiedDomain}",
                Html = body
            });

            Assert.NotEmpty(message.Id);
            Assert.Contains(body, message.Html.Body);
        }

        [Fact]
        public void ReplyTextTest()
        {
            // TODO When xUnit 3 is released, use Assert.SkipIf
            if (string.IsNullOrWhiteSpace(this.fixture.verifiedDomain))
            {
                return;
            }

            var body = "Reply message";
            var targetEmail = this.fixture.emails[0];

            var message = this.fixture.client.Messages.Reply(targetEmail.Id, new MessageReplyOptions()
            {
                Text = body
            });

            Assert.NotEmpty(message.Id);
            Assert.Contains(body, message.Text.Body);
        }

        [Fact]
        public void ReplyHtmlTest()
        {
            // TODO When xUnit 3 is released, use Assert.SkipIf
            if (string.IsNullOrWhiteSpace(this.fixture.verifiedDomain))
            {
                return;
            }

            var body = "<p>Reply <strong>HTML</strong> message.</p>";
            var targetEmail = this.fixture.emails[0];

            var message = this.fixture.client.Messages.Reply(targetEmail.Id, new MessageReplyOptions()
            {
                Html = body
            });

            Assert.NotEmpty(message.Id);
            Assert.Contains(body, message.Html.Body);
        }

        [Fact]
        public void ReplyWithAttachment()
        {
            // TODO When xUnit 3 is released, use Assert.SkipIf
            if (string.IsNullOrWhiteSpace(this.fixture.verifiedDomain))
            {
                return;
            }

            var body = "<p>Reply with attachment.</p>";
            var targetEmail = this.fixture.emails[0];

            var data = File.ReadAllBytes(Path.Combine("Resources", "cat.png"));
            var attachment = new Attachment()
            {
                FileName = "cat.png",
                Content = Convert.ToBase64String(data),
                ContentType = "image/png"
            };

            var message = this.fixture.client.Messages.Reply(targetEmail.Id, new MessageReplyOptions()
            {
                Html = body,
                Attachments = new List<Attachment>() { attachment }
            });

            Assert.Equal(1, message.Attachments.Count);
            var file1 = message.Attachments[0];
            Assert.NotNull(file1.Id);
            Assert.Equal(82138, file1.Length);
            Assert.NotNull(file1.Url);
            Assert.Equal("cat.png", file1.FileName);
            Assert.Equal("image/png", file1.ContentType);
        }

        private void ValidateEmail(Message email)
        {
            ValidateMetadata(email);
            ValidateAttachmentMetadata(email);
            ValidateHtml(email);
            ValidateText(email);
            Assert.NotEmpty(email.Metadata.Ehlo);
            Assert.NotEmpty(email.Metadata.MailFrom);
            Assert.Equal(1, email.Metadata.RcptTo.Count());
        }

        private void ValidateEmailSummary(MessageSummary email)
        {
            ValidateMetadata(email);
            Assert.NotEmpty(email.Summary);
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

            // Html.Codes
            Assert.Equal(2, email.Html.Codes.Count);
            Assert.Equal("123456", email.Html.Codes[0].Value);
            Assert.Equal("G3H1Y2", email.Html.Codes[1].Value);

            // Html.Images
            Assert.StartsWith("cid:", email.Html.Images[1].Src);
            Assert.Equal("Inline image 1", email.Html.Images[1].Alt);
        }

        private void ValidateText(Message email)
        {
            // Text.Body
            Assert.StartsWith("this is a test", email.Text.Body);

            // Text.Links
            Assert.Equal(2, email.Text.Links.Count);
            Assert.Equal("https://mailosaur.com/", email.Text.Links[0].Href);
            Assert.Equal(email.Text.Links[0].Href, email.Text.Links[0].Text);
            Assert.Equal("https://mailosaur.com/", email.Text.Links[1].Href);
            Assert.Equal(email.Text.Links[1].Href, email.Text.Links[1].Text);

            // Text.Codes
            Assert.Equal(2, email.Text.Codes.Count);
            Assert.Equal("654321", email.Text.Codes[0].Value);
            Assert.Equal("5H0Y2", email.Text.Codes[1].Value);
        }

        private void ValidateHeaders(Message email)
        {
            var expectedFromHeader = $"\"{email.From[0].Name}\" <{email.From[0].Email}>";
            var expectedToHeader = $"\"{email.To[0].Name}\" <{email.To[0].Email}>";

            // Fallback casing is used, as header casing is determined by sending server
            // Assert.Equal(expectedFromHeader, actual.Headers.ContainsKey("From") ?
            //     actual.Headers["From"].ToString() : actual.Headers["from"].ToString());

            // Assert.Equal(expectedToHeader, actual.Headers.ContainsKey("To") ?
            //     actual.Headers["To"].ToString() : actual.Headers["to"].ToString());

            // Assert.Equal(expected.Subject, actual.Headers.ContainsKey("Subject") ?
            //     actual.Headers["Subject"].ToString() : actual.Headers["subject"].ToString());
        }

        private void ValidateMetadata(Message email)
        {
            ValidateMetadata(new MessageSummary()
            {
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
            Assert.Equal("dog.png", file2.FileName);
            Assert.Equal("image/png", file2.ContentType);
        }
    }
}
