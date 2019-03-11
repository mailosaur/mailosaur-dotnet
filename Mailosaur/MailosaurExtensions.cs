using System.IO;

namespace Mailosaur
{
    public static partial class MailosaurExtensions
    {
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
