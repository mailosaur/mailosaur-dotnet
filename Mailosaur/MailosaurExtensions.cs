using System;
using System.IO;
using Microsoft.Rest;

namespace Mailosaur
{
    public partial class MailosaurClient : ServiceClient<MailosaurClient>, IMailosaurClient
    {
        public MailosaurClient(string apiKey) :
            this(new BasicAuthenticationCredentials { UserName = apiKey }) {}

        public MailosaurClient(string apiKey, string baseUrl) :
            this(new Uri(baseUrl), new BasicAuthenticationCredentials { UserName = apiKey }) {}
    }

    /// <summary>
    /// Custom non auto generated extension methods for MailosaurAPI.
    /// </summary>
    public static partial class MailosaurExtensions
    {
        /// <summary>
        /// Generates a random email address for use with this server.
        /// </summary>
        /// <returns>A random new email address that will end up in this server.</returns>
        /// <param name="operations">Operations.</param>
        /// <param name="serverId">Server identifier.</param>
        /// <param name="host">Host.</param>
        public static string GenerateEmailAddress(this IServers operations, string serverId)
        {
            string host = Environment.GetEnvironmentVariable("MAILOSAUR_SMTP_HOST") ?? "mailosaur.io";
            string guid = Guid.NewGuid().ToString();
            return string.Format("{0}.{1}@" + host, guid, serverId);
        }

        /// <summary>
        /// Reads a stream into a byte array.
        /// </summary>
        /// <returns>The contents of the stream as a byte array.</returns>
        /// <param name="stream">Stream.</param>
        public static byte[] ReadToArray(this Stream stream)
        {
            using (var memStream = new MemoryStream())
            {
                stream.CopyTo(memStream);
                return memStream.ToArray();
            }
        }
    }
}
