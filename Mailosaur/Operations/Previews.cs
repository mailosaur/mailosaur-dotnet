namespace Mailosaur.Operations
{
    using Models;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Previews operations.
    /// </summary>
    public class Previews : OperationBase
    {
        /// <summary>
        /// Initializes a new instance of the Previews class.
        /// </summary>
        /// <param name='client'>
        /// Reference to the HttpClient.
        /// </param>
        public Previews(HttpClient client) : base(client) { }

        /// <summary>
        /// List all email preview clients
        /// </summary>
        /// <remarks>
        /// Returns the list of all email clients that can be used to generate email previews.
        /// </remarks>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public PreviewEmailClientListResult ListEmailClients()
            => Task.Run(async () => await ListEmailClientsAsync()).UnwrapException<PreviewEmailClientListResult>();

        /// <summary>
        /// List all email preview clients
        /// </summary>
        /// <remarks>
        /// Returns the list of all email clients that can be used to generate email previews.
        /// </remarks>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public Task<PreviewEmailClientListResult> ListEmailClientsAsync()
            => ExecuteRequest<PreviewEmailClientListResult>(HttpMethod.Get, $"api/previews/clients");
    }
}
