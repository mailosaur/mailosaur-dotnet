using System;
using System.Linq;
using Mailosaur.Models;
using Xunit;

namespace Mailosaur.Test
{
    public class UsageTests : IDisposable
    {
        private MailosaurClient m_Client;
        private string s_BaseUrl = Environment.GetEnvironmentVariable("MAILOSAUR_BASE_URL") ?? "https://mailosaur.com/";

        public UsageTests()
        {
            m_Client = new MailosaurClient(baseUrl: s_BaseUrl);
        }

        [Fact]
        public void LimitsTest()
        {
            UsageAccountLimits result = m_Client.Usage.Limits();
            Assert.NotNull(result.Servers);
            Assert.NotNull(result.Users);
            Assert.NotNull(result.Email);
            Assert.NotNull(result.Sms);

            Assert.True(result.Servers.Limit > 0);
            Assert.True(result.Users.Limit > 0);
            Assert.True(result.Email.Limit > 0);
            Assert.True(result.Sms.Limit > 0);
        }

        [Fact]
        public void TransactionsTest()
        {
            UsageTransactionListResult result = m_Client.Usage.Transactions();
            Assert.True(result.Items.Count() > 1);
            Assert.IsType<DateTime>(result.Items[0].Timestamp);
            Assert.IsType<int>(result.Items[0].Email);
            Assert.IsType<int>(result.Items[0].Sms);
        }

        public void Dispose()
        {
            m_Client.Dispose();
        }
    }
}
