namespace Mailosaur.Operations
{
    using System;
    using System.IO;
    using System.Net;
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
        public Files(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Gets a reference to the HttpClient
        /// </summary>
        private readonly HttpClient _client;

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
        {
            try
            {
                return GetAttachmentAsync(id).Result;
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
        public async Task<Stream> GetAttachmentAsync(string id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/files/attachments/" + id);
            var response = await _client.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
                await ThrowExceptionAsync(response);

            request.Dispose();

            return await response.Content.ReadAsStreamAsync();
        }

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
        {
            try
            {
                return GetEmailAsync(id).Result;
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
        public async Task<Stream> GetEmailAsync(string id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/files/email/" + id);
            var response = await _client.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
                await ThrowExceptionAsync(response);

            request.Dispose();

            return await response.Content.ReadAsStreamAsync();
        }
    }
}
