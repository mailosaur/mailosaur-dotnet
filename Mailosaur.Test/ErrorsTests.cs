using System;
using Mailosaur.Models;
using Xunit;

namespace Mailosaur.Test
{
    public class ErrorsTests
    {
        private static string s_ApiKey = Environment.GetEnvironmentVariable("MAILOSAUR_API_KEY");
        private string s_BaseUrl = Environment.GetEnvironmentVariable("MAILOSAUR_BASE_URL") ?? "https://mailosaur.com/";

        [Fact]
        public void UnauthorizedTest()
        {
            using (var client = new MailosaurClient("invalid_key", s_BaseUrl))
            {
                var ex = Assert.Throws<MailosaurException>(delegate
                {
                    client.Servers.List();
                });

                Assert.Equal("Authentication failed, check your API key.", ex.Message);
            }
        }

        [Fact]
        public void NotFoundTest()
        {
            using (var client = new MailosaurClient(s_ApiKey, s_BaseUrl))
            {
                var ex = Assert.Throws<MailosaurException>(delegate
                {
                    client.Servers.Get("not_found");
                });

                Assert.Equal("Not found, check input parameters.", ex.Message);
            }
        }

        [Fact]
        public void BadRequestTest()
        {
            using (var client = new MailosaurClient(s_ApiKey, s_BaseUrl))
            {
                var options = new ServerCreateOptions("");
                var ex = Assert.Throws<MailosaurException>(delegate
                {
                    client.Servers.Create(options);
                });

                Assert.Equal("(name) Servers need a name\r\n", ex.Message);
            }
        }
    }
}
