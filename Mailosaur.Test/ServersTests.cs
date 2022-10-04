using System;
using System.Linq;
using Mailosaur.Models;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mailosaur.Test
{
    public class ServersTests : IDisposable
    {
        private MailosaurClient m_Client;
        private static string s_ApiKey = Environment.GetEnvironmentVariable("MAILOSAUR_API_KEY");
        private string s_BaseUrl = Environment.GetEnvironmentVariable("MAILOSAUR_BASE_URL") ?? "https://mailosaur.com/";

        public ServersTests()
        {
            if (string.IsNullOrWhiteSpace(s_ApiKey))
            {
                throw new Exception("Missing necessary environment variables - refer to README.md");
            }

            m_Client = new MailosaurClient(s_ApiKey, s_BaseUrl);
        }

        [Fact]
        public void ListTest()
        {
            IList<Server> servers = m_Client.Servers.List().Items;
            Assert.True(servers.Count() > 1);
        }

        [Fact]
        public void GetNotFoundTest()
        {
            // Should throw if server is not found
            Assert.Throws<MailosaurException>(delegate
            {
                m_Client.Servers.Get("efe907e9-74ed-4113-a3e0-a3d41d914765");
            });
        }

        [Fact]
        public void CrudTest()
        {
            var serverName = "My test";

            // Create a new server
            var options = new ServerCreateOptions(serverName);
            Server createdServer = m_Client.Servers.Create(options);
            Assert.False(string.IsNullOrWhiteSpace(createdServer.Id));
            Assert.Equal(serverName, createdServer.Name);
            Assert.NotNull(createdServer.Users);
            Assert.Equal(0, createdServer.Messages);

            // Retrieve a server and confirm it has expected content
            Server retrievedServer = m_Client.Servers.Get(createdServer.Id);
            Assert.Equal(createdServer.Id, retrievedServer.Id);
            Assert.Equal(createdServer.Name, retrievedServer.Name);
            Assert.NotNull(retrievedServer.Users);
            Assert.Equal(0, retrievedServer.Messages);

            // Update a server and confirm it has changed
            retrievedServer.Name += " updated with ellipsis … and emoji 👨🏿‍🚒";
            Server updatedServer = m_Client.Servers.Update(retrievedServer.Id, retrievedServer);
            Assert.Equal(retrievedServer.Id, updatedServer.Id);
            Assert.Equal(retrievedServer.Name, updatedServer.Name);
            Assert.Equal(retrievedServer.Users, updatedServer.Users);
            Assert.Equal(retrievedServer.Messages, updatedServer.Messages);

            m_Client.Servers.Delete(retrievedServer.Id);

            // Attempting to delete again should fail
            Assert.Throws<MailosaurException>(delegate
            {
                m_Client.Servers.Delete(retrievedServer.Id);
            });
        }

        [Fact]
        public void FailedCreateTest()
        {
            var options = new ServerCreateOptions("");

            var ex = Assert.Throws<MailosaurException>(delegate
            {
                m_Client.Servers.Create(options);
            });

            Assert.Equal("(name) Servers need a name\r\n", ex.Message);
            Assert.Equal("invalid_request", ex.ErrorType);
            Assert.Equal(400, ex.HttpStatusCode);
            Assert.Contains("{\"type\":", ex.HttpResponseBody);
        }

        public void Dispose()
        {
            m_Client.Dispose();
        }
    }
}
