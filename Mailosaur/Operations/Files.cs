namespace Mailosaur.Operations
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Mailosaur.Models;

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

        /// <summary>
        /// Download Email Preview
        /// </summary>
        /// <remarks>
        /// Downloads a screenshot of your email rendered in a real email client. Simply supply 
        /// the unique identifier for the required preview.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the preview to be downloaded.
        /// </param>
        public byte[] GetPreview(string id)
            => Task.Run(async () => await GetPreviewAsync(id)).UnwrapException<byte[]>();

        /// <summary>
        /// Download Email Preview
        /// </summary>
        /// <remarks>
        /// Downloads a screenshot of your email rendered in a real email client. Simply supply 
        /// the unique identifier for the required preview.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the preview to be downloaded.
        /// </param>
        public Task<byte[]> GetPreviewAsync(string id)
            => GetScreenshotAsync(id);

        private async Task<byte[]> GetScreenshotAsync(string id)
        {
            var timeout = 120000;
            var pollCount = 0;
            var startTime = DateTime.UtcNow;

            while (true)
            {
                var response = await ExecuteRequestWithResponse(HttpMethod.Get, $"api/files/screenshots/{id}");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }

                response.Headers.TryGetValues("x-ms-delay", out var delayHeaderValues);
                var delayString = delayHeaderValues?.FirstOrDefault() ?? "1000";
                var delayPattern = delayString.Split(',').Select(x => int.Parse(x.Trim())).ToArray();

                var delay = pollCount >= delayPattern.Length ?
                    delayPattern[delayPattern.Length - 1] :
                    delayPattern[pollCount];

                pollCount++;

                // Stop if timeout will be exceeded
                if (((int)(DateTime.UtcNow - startTime).TotalMilliseconds) + delay > timeout)
                {
                    throw new MailosaurException(
                        $"An email preview was not generated in time. The email client may not be available, or the preview ID [{id}] may be incorrect.",
                        "preview_timeout");
                }

                Task.Delay(delay).Wait();
            }
        }
    }
}
