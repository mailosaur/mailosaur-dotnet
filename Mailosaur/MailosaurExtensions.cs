using System;
using System.IO;
using System.Threading.Tasks;

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

        public static void WaitAndUnwrapException(this Task task)
        {
            try
            {
                task.Wait();
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static T UnwrapException<T>(this Task<T> task)
        {
            try
            {
                return task.Result;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return default(T);
        }
    }
}
