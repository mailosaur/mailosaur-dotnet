using System;
using System.Linq;
using Mailosaur.Models;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mailosaur.Test
{
    public class DevicesTests
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

            Assert.Equal(1, m_Client.Devices.List().Items.Count());
            m_Client.Devices.Delete(createdDevice.Id);
            Assert.Equal(0, m_Client.Devices.List().Items.Count());
        }

        [Fact]
        public void OtpViaSharedSecretTest()
        {
            var sharedSecret = "ONSWG4TFOQYTEMY=";
            OtpResult otpResult = m_Client.Devices.Otp(sharedSecret);
            Assert.Equal(6, otpResult.Code.Length);
        }
    }
}
