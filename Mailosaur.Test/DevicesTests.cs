using System;
using System.Linq;
using Mailosaur.Models;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mailosaur.Test
{
    public class DevicesTests : IDisposable
    {
        private MailosaurClient m_Client;
        private static string s_ApiKey = Environment.GetEnvironmentVariable("MAILOSAUR_API_KEY");
        private string s_BaseUrl = Environment.GetEnvironmentVariable("MAILOSAUR_BASE_URL") ?? "https://mailosaur.com/";

        public DevicesTests()
        {
            if (string.IsNullOrWhiteSpace(s_ApiKey))
            {
                throw new Exception("Missing necessary environment variables - refer to README.md");
            }

            m_Client = new MailosaurClient(s_ApiKey, s_BaseUrl);
        }

        [Fact]
        public void CrudTest()
        {
            var deviceName = "My test";
            var sharedSecret = "ONSWG4TFOQYTEMY=";

            // Create a new device
            var options = new DeviceCreateOptions(deviceName, sharedSecret);
            Device createdDevice = m_Client.Devices.Create(options);
            Assert.False(string.IsNullOrWhiteSpace(createdDevice.Id));
            Assert.Equal(deviceName, createdDevice.Name);

            // Retrieve an otp via device ID
            OtpResult otpResult = m_Client.Devices.Otp(createdDevice.Id);
            Assert.Equal(6, otpResult.Code.Length);

            var before = m_Client.Devices.List();
            Assert.True(before.Items.Any(x => x.Id == createdDevice.Id));

            m_Client.Devices.Delete(createdDevice.Id);

            var after = m_Client.Devices.List();
            Assert.False(after.Items.Any(x => x.Id == createdDevice.Id));
        }

        [Fact]
        public void OtpViaSharedSecretTest()
        {
            var sharedSecret = "ONSWG4TFOQYTEMY=";
            OtpResult otpResult = m_Client.Devices.Otp(sharedSecret);
            Assert.Equal(6, otpResult.Code.Length);
        }

        public void Dispose()
        {
            m_Client.Dispose();
        }
    }
}
