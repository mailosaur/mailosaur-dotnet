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
        public Stream GetAttachment(string id)
            => HandleAggregateException<Stream>(() => GetAttachmentAsync(id).Result);

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
        public Task<Stream> GetAttachmentAsync(string id)
            => ExecuteStreamRequest(HttpMethod.Get, $"api/files/attachments/{id}");

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
        public Stream GetEmail(string id)
            => HandleAggregateException<Stream>(() => GetEmailAsync(id).Result);

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
        public Task<Stream> GetEmailAsync(string id)
            => ExecuteStreamRequest(HttpMethod.Get, $"api/files/email/{id}");
    }
}
