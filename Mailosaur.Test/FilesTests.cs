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
    [Collection("Mailosaur Client")]
    public class FilesTests
    {
        EmailsFixture fixture;
        
        public FilesTests(EmailsFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void GetEmailTest()
        {
            Message email = this.fixture.emails[0];

            Stream stream = this.fixture.m_Client.Files.GetEmail(email.Id);
            byte[] bytes = stream.ReadToArray();
            
            Assert.NotNull(bytes);
            Assert.True(bytes.Length > 1);
            Assert.Contains(email.Subject, Encoding.UTF8.GetString(bytes));
        }

        [Fact]
        public void GetAttachmentTest()
        {
            Message email = this.fixture.emails[0];
            Attachment attachment = email.Attachments[0];

            Stream stream = this.fixture.m_Client.Files.GetAttachment(attachment.Id);
            byte[] bytes = stream.ReadToArray();
            
            Assert.NotNull(bytes);
            Assert.Equal(attachment.Length, bytes.Length);
        }
    }
}