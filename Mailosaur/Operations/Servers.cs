namespace Mailosaur.Operations
{
    using Models;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using System;
    using System.Text;

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
        public Servers(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Gets a reference to the Http_client
        /// </summary>
        private readonly HttpClient _client;

        /// <summary>
        /// Generates a random email address for use with this server.
        /// </summary>
        /// <returns>A random new email address that will end up in this server.</returns>
        /// <param name="operations">Operations.</param>
        /// <param name="serverId">Server identifier.</param>
        /// <param name="host">Host.</param>
        public string GenerateEmailAddress(string serverId)
        {
            string host = Environment.GetEnvironmentVariable("MAILOSAUR_SMTP_HOST") ?? "mailosaur.io";
            string guid = Guid.NewGuid().ToString();
            return string.Format("{0}.{1}@" + host, guid, serverId);
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
        {
            try
            {
                return ListAsync().Result;
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
        public async Task<ServerListResult> ListAsync()
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, "api/servers"))
            using (var response = await _client.SendAsync(request))
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    await ThrowExceptionAsync(response);

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ServerListResult>(content);
            }
        }

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
        {
            try
            {
                return CreateAsync(serverCreateOptions).Result;
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
        public async Task<Server> CreateAsync(ServerCreateOptions serverCreateOptions)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, "api/servers"))
            {
                var requestContent = JsonConvert.SerializeObject(serverCreateOptions);
                request.Content = new StringContent(requestContent, Encoding.UTF8, "application/json");
                using (var response = await _client.SendAsync(request))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        await ThrowExceptionAsync(response);

                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Server>(content);
                }
            }
        }

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
        {
            try
            {
                return GetAsync(id).Result;
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
        public async Task<Server> GetAsync(string id)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, "api/servers/" + id))
            using (var response = await _client.SendAsync(request))
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    await ThrowExceptionAsync(response);

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Server>(content);
            }
        }

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
        {
            try
            {
                return UpdateAsync(id, server).Result;
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
        public async Task<Server> UpdateAsync(string id, Server server)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Put, "api/servers/" + id))
            {
                var requestContent = JsonConvert.SerializeObject(server);
                request.Content = new StringContent(requestContent, Encoding.UTF8, "application/json");
                using (var response = await _client.SendAsync(request))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        await ThrowExceptionAsync(response);

                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Server>(content);
                }
            }
        }

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
        {
            try
            {
                DeleteAsync(id).Wait();
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
        public async Task DeleteAsync(string id)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Delete, "api/servers/" + id))
            using (var response = await _client.SendAsync(request))
            {
                if (response.StatusCode != HttpStatusCode.NoContent)
                    await ThrowExceptionAsync(response);
            }
        }
    }
}
