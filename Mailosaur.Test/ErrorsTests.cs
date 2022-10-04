using System;
using Mailosaur.Models;
using Xunit;

namespace Mailosaur.Test
{
    public class ErrorsTests
    {
        [Fact]
        public void UnauthorizedTest()
        {
            MailosaurException ex;
            using (var client = new MailosaurClient("invalid_key"))
            {
                ex = Assert.Throws<MailosaurException>(delegate { client.Servers.List(); });
            }

            Assert.Equal("Authentication failed, check your API key.", ex.Message);
        }

        [Fact]
        public void NotFoundTest()
        {
            MailosaurException ex;
            using (var client = new MailosaurClient(Environment.GetEnvironmentVariable("MAILOSAUR_API_KEY")))
            {
                ex = Assert.Throws<MailosaurException>(delegate { client.Servers.Get("not_found"); });
            }

            Assert.Equal("Not found, check input parameters.", ex.Message);
        }

        [Fact]
        public void BadRequestTest()
        {
            MailosaurException ex;
            using (var client = new MailosaurClient(Environment.GetEnvironmentVariable("MAILOSAUR_API_KEY")))
            {
                var options = new ServerCreateOptions("");
                ex = Assert.Throws<MailosaurException>(delegate { client.Servers.Create(options); });
            }

            Assert.Equal("(name) Servers need a name\r\n", ex.Message);
        }
    }
}
