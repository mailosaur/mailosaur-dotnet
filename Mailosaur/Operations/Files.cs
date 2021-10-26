namespace Mailosaur.Operations
{
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class Files : OperationBase
    {
        /// <summary>
        /// Initializes a new instance of the Files class.
        /// </summary>
        /// <param name='client'>
        /// Reference to the http client.
        /// </param>
        public Files(HttpClient client) : base(client) { }

        /// <summary>
        /// Download an attachment
        /// </summary>
        /// <remarks>
        /// Downloads a single attachment. Simply supply the unique identifier for the
        /// required attachment.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the attachment to be downloaded.
        /// </param>
        public byte[] GetAttachment(string id)
            => Task.Run(async () => await GetAttachmentAsync(id)).UnwrapException<byte[]>();

        /// <summary>
        /// Download an attachment
        /// </summary>
        /// <remarks>
        /// Downloads a single attachment. Simply supply the unique identifier for the
        /// required attachment.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the attachment to be downloaded.
        /// </param>
        public Task<byte[]> GetAttachmentAsync(string id)
            => ExecuteBytesRequest(HttpMethod.Get, $"api/files/attachments/{id}");

        /// <summary>
        /// Download EML
        /// </summary>
        /// <remarks>
        /// Downloads an EML file representing the specified email. Simply supply the
        /// unique identifier for the required email.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the email to be downloaded.
        /// </param>
        public byte[] GetEmail(string id)
            => Task.Run(async () => await GetEmailAsync(id)).UnwrapException<byte[]>();

        /// <summary>
        /// Download EML
        /// </summary>
        /// <remarks>
        /// Downloads an EML file representing the specified email. Simply supply the
        /// unique identifier for the required email.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the email to be downloaded.
        /// </param>
        public Task<byte[]> GetEmailAsync(string id)
            => ExecuteBytesRequest(HttpMethod.Get, $"api/files/email/{id}");
    }
}
