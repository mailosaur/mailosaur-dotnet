namespace Mailosaur.Operations
{
    using Models;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Operations for discovering the email clients available for generating email previews
    /// (screenshots of an email rendered in real clients). Accessed via <c>client.Previews</c>.
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
        /// <returns>
        /// An <see cref="EmailClientListResult"/> of available email clients.
        /// </returns>
        public EmailClientListResult ListEmailClients()
            => Task.Run(async () => await ListEmailClientsAsync()).UnwrapException<EmailClientListResult>();

        /// <summary>
        /// List all email preview clients
        /// </summary>
        /// <remarks>
        /// Returns the list of all email clients that can be used to generate email previews.
        /// </remarks>
        /// <returns>
        /// A task resolving to an <see cref="EmailClientListResult"/> of available email clients.
        /// </returns>
        public Task<EmailClientListResult> ListEmailClientsAsync()
            => ExecuteRequest<EmailClientListResult>(HttpMethod.Get, $"api/screenshots/clients");
    }
}
