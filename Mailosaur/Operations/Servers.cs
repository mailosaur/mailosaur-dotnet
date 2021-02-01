namespace Mailosaur.Operations
{
    using Models;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System;

    /// <summary>
    /// Servers operations.
    /// </summary>
    public class Servers : OperationBase
    {
        /// <summary>
        /// Initializes a new instance of the Servers class.
        /// </summary>
        /// <param name='client'>
        /// Reference to the HttpClient.
        /// </param>
        public Servers(HttpClient client) : base(client) { }

        /// <summary>
        /// Generates a random email address for use with this server.
        /// </summary>
        /// <param name="serverId">Server identifier.</param>
        /// <returns>A random new email address that will end up in this server.</returns>
        public string GenerateEmailAddress(string serverId)
        {
            string host = Environment.GetEnvironmentVariable("MAILOSAUR_SMTP_HOST") ?? "mailosaur.net";
            string guid = Guid.NewGuid().ToString();
            return $"{guid}@{serverId}.{host}";
        }

        /// <summary>
        /// List all servers
        /// </summary>
        /// <remarks>
        /// Returns a list of your virtual SMTP servers. Servers are returned sorted in
        /// alphabetical order.
        /// </remarks>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public ServerListResult List()
            => Task.Run(async () => await ListAsync()).UnwrapException<ServerListResult>();

        /// <summary>
        /// List all servers
        /// </summary>
        /// <remarks>
        /// Returns a list of your virtual SMTP servers. Servers are returned sorted in
        /// alphabetical order.
        /// </remarks>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public Task<ServerListResult> ListAsync() 
            => ExecuteRequest<ServerListResult>(HttpMethod.Get, $"api/servers");

        /// <summary>
        /// Create a server
        /// </summary>
        /// <remarks>
        /// Creates a new virtual SMTP server and returns it.
        /// </remarks>
        /// <param name='serverCreateOptions'>
        /// </param>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public Server Create(ServerCreateOptions serverCreateOptions)
            => Task.Run(async () => await CreateAsync(serverCreateOptions)).UnwrapException<Server>();

        /// <summary>
        /// Create a server
        /// </summary>
        /// <remarks>
        /// Creates a new virtual SMTP server and returns it.
        /// </remarks>
        /// <param name='serverCreateOptions'>
        /// </param>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public Task<Server> CreateAsync(ServerCreateOptions serverCreateOptions) 
            => ExecuteRequest<Server>(HttpMethod.Post, "api/servers", serverCreateOptions);

        /// <summary>
        /// Retrieve a server
        /// </summary>
        /// <remarks>
        /// Retrieves the detail for a single server. Simply supply the unique
        /// identifier for the required server.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the server to be retrieved.
        /// </param>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public Server Get(string id)
            => Task.Run(async () => await GetAsync(id)).UnwrapException<Server>();

        /// <summary>
        /// Retrieve a server
        /// </summary>
        /// <remarks>
        /// Retrieves the detail for a single server. Simply supply the unique
        /// identifier for the required server.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the server to be retrieved.
        /// </param>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public Task<Server> GetAsync(string id) 
            => ExecuteRequest<Server>(HttpMethod.Get, $"api/servers/{id}");

        /// <summary>
        /// Update a server
        /// </summary>
        /// <remarks>
        /// Updats a single server and returns it.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the server to be updated.
        /// </param>
        /// <param name='server'>
        /// </param>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public Server Update(string id, Server server)
            => Task.Run(async () => await UpdateAsync(id, server)).UnwrapException<Server>();

        /// <summary>
        /// Update a server
        /// </summary>
        /// <remarks>
        /// Updats a single server and returns it.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the server to be updated.
        /// </param>
        /// <param name='server'>
        /// </param>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public Task<Server> UpdateAsync(string id, Server server) 
            => ExecuteRequest<Server>(HttpMethod.Put, $"api/servers/{id}", server);

        /// <summary>
        /// Delete a server
        /// </summary>
        /// <remarks>
        /// Permanently deletes a server. This operation cannot be undone. Also deletes
        /// all messages and associated attachments within the server.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the server to be deleted.
        /// </param>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public void Delete(string id)
            => Task.Run(async () => await DeleteAsync(id)).WaitAndUnwrapException();

        /// <summary>
        /// Delete a server
        /// </summary>
        /// <remarks>
        /// Permanently deletes a server. This operation cannot be undone. Also deletes
        /// all messages and associated attachments within the server.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the server to be deleted.
        /// </param>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public Task DeleteAsync(string id)
            => ExecuteRequest(HttpMethod.Delete, $"api/servers/{id}");
    }
}
