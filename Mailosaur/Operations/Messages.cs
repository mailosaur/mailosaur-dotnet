namespace Mailosaur.Operations
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Mailosaur.Models;
    using Newtonsoft.Json;

    public class Messages : OperationBase
    {
        /// <summary>
        /// Initializes a new instance of the Messages class.
        /// </summary>
        /// <param name='client'>
        /// Reference to the HttpClient.
        /// </param>
        public Messages(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Gets a reference to the Http_client
        /// </summary>
        private readonly HttpClient _client;

        /// <summary>
        /// Retrieve a message
        /// </summary>
        /// <remarks>
        /// Retrieves the detail for a single email message. Simply supply the unique
        /// identifier for the required message.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the email message to be retrieved.
        /// </param>
        public Message Get(string id)
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
        /// Retrieve a message
        /// </summary>
        /// <remarks>
        /// Retrieves the detail for a single email message. Simply supply the unique
        /// identifier for the required message.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the email message to be retrieved.
        /// </param>
        public async Task<Message> GetAsync(string id)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, "api/messages/" + id))
            using (var response = await _client.SendAsync(request))
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    await ThrowExceptionAsync(response);

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Message>(content);
            }
        }

        /// <summary>
        /// Delete a message
        /// </summary>
        /// <remarks>
        /// Permanently deletes a message. This operation cannot be undone. Also
        /// deletes any attachments related to the message.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the message to be deleted.
        /// </param>
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
        /// Delete a message
        /// </summary>
        /// <remarks>
        /// Permanently deletes a message. This operation cannot be undone. Also
        /// deletes any attachments related to the message.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the message to be deleted.
        /// </param>
        public async Task DeleteAsync(string id)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Delete, "api/messages/" + id))
            using (var response = await _client.SendAsync(request))
            {
                if (response.StatusCode != HttpStatusCode.NoContent)
                    await ThrowExceptionAsync(response);
            }
        }

        /// <summary>
        /// List all messages
        /// </summary>
        /// <remarks>
        /// Returns a list of your messages in summary form. The summaries are returned
        /// sorted by received date, with the most recently-received messages appearing
        /// first.
        /// </remarks>
        /// <param name='server'>
        /// The identifier of the server hosting the messages.
        /// </param>
        /// <param name='page'>
        /// Used in conjunction with `itemsPerPage` to support pagination.
        /// </param>
        /// <param name='itemsPerPage'>
        /// A limit on the number of results to be returned per page. Can be set
        /// between 1 and 1000 items, the default is 50.
        /// </param>
        public MessageListResult List(string server, int? page = default(int?), int? itemsPerPage = default(int?))
        {
            try
            {
                return ListAsync(server, page, itemsPerPage).Result;
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
        /// List all messages
        /// </summary>
        /// <remarks>
        /// Returns a list of your messages in summary form. The summaries are returned
        /// sorted by received date, with the most recently-received messages appearing
        /// first.
        /// </remarks>
        /// <param name='server'>
        /// The identifier of the server hosting the messages.
        /// </param>
        /// <param name='page'>
        /// Used in conjunction with `itemsPerPage` to support pagination.
        /// </param>
        /// <param name='itemsPerPage'>
        /// A limit on the number of results to be returned per page. Can be set
        /// between 1 and 1000 items, the default is 50.
        /// </param>
        public async Task<MessageListResult> ListAsync(string server, int? page = default(int?), int? itemsPerPage = default(int?))
        {
            var url = "api/messages?server=" + server;
            url += page != null ? "&page=" + page : "";
            url += itemsPerPage != null ? "&itemsPerPage=" + itemsPerPage : "";
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            using (var response = await _client.SendAsync(request))
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    await ThrowExceptionAsync(response);

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<MessageListResult>(content);
            }
        }

        /// <summary>
        /// Delete all messages
        /// </summary>
        /// <remarks>
        /// Permanently deletes all messages held by the specified server. This
        /// operation cannot be undone. Also deletes any attachments related to each
        /// message.
        /// </remarks>
        /// <param name='server'>
        /// The identifier of the server to be emptied.
        /// </param>
        public void DeleteAll(string server)
        {
            try
            {
                DeleteAllAsync(server).Wait();
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
        /// Delete all messages
        /// </summary>
        /// <remarks>
        /// Permanently deletes all messages held by the specified server. This
        /// operation cannot be undone. Also deletes any attachments related to each
        /// message.
        /// </remarks>
        /// <param name='server'>
        /// The identifier of the server to be emptied.
        /// </param>
        public async Task DeleteAllAsync(string server)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Delete, "api/messages?server=" + server))
            using (var response = await _client.SendAsync(request))
            {
                if (response.StatusCode != HttpStatusCode.NoContent)
                    await ThrowExceptionAsync(response);
            }
        }

        /// <summary>
        /// Search for messages
        /// </summary>
        /// <remarks>
        /// Returns a list of messages matching the specified search criteria, in
        /// summary form. The messages are returned sorted by received date, with the
        /// most recently-received messages appearing first.
        /// </remarks>
        /// <param name='server'>
        /// The identifier of the server hosting the messages.
        /// </param>
        /// <param name='criteria'>
        /// The search criteria to match results against.
        /// </param>
        /// <param name='page'>
        /// Used in conjunction with `itemsPerPage` to support pagination.
        /// </param>
        /// <param name='itemsPerPage'>
        /// A limit on the number of results to be returned per page. Can be set
        /// between 1 and 1000 items, the default is 50.
        /// </param>
        public MessageListResult Search(string server, SearchCriteria criteria, int? page = null, int? itemsPerPage = null)
        {
            try
            {
                return SearchAsync(server, criteria, page, itemsPerPage).Result;
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
        /// Search for messages
        /// </summary>
        /// <remarks>
        /// Returns a list of messages matching the specified search criteria, in
        /// summary form. The messages are returned sorted by received date, with the
        /// most recently-received messages appearing first.
        /// </remarks>
        /// <param name='server'>
        /// The identifier of the server hosting the messages.
        /// </param>
        /// <param name='criteria'>
        /// The search criteria to match results against.
        /// </param>
        /// <param name='page'>
        /// Used in conjunction with `itemsPerPage` to support pagination.
        /// </param>
        /// <param name='itemsPerPage'>
        /// A limit on the number of results to be returned per page. Can be set
        /// between 1 and 1000 items, the default is 50.
        /// </param>
        public async Task<MessageListResult> SearchAsync(string server, SearchCriteria criteria, int? page = null, int? itemsPerPage = null)
        {
            var url = "api/messages/search?server=" + server;
            url += page != null ? "&page=" + page : "";
            url += itemsPerPage != null ? "&itemsPerPage=" + itemsPerPage : "";
            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                var requestContent = JsonConvert.SerializeObject(criteria);
                request.Content = new StringContent(requestContent, Encoding.UTF8, "application/json");
                using (var response = await _client.SendAsync(request))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        await ThrowExceptionAsync(response);

                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MessageListResult>(content);
                }
            }
        }

        /// <summary>
        /// Wait for a specific message
        /// </summary>
        /// <remarks>
        /// Returns as soon as a message matching the specified search criteria is
        /// found. This is the most efficient method of looking up a message.
        /// </remarks>
        /// <param name='server'>
        /// The identifier of the server hosting the message.
        /// </param>
        /// <param name='criteria'>
        /// The search criteria to use in order to find a match.
        /// </param>
        public Message WaitFor(string server, SearchCriteria criteria)
        {
            try
            {
                return WaitForAsync(server, criteria).Result;
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
        /// Wait for a specific message
        /// </summary>
        /// <remarks>
        /// Returns as soon as a message matching the specified search criteria is
        /// found. This is the most efficient method of looking up a message.
        /// </remarks>
        /// <param name='server'>
        /// The identifier of the server hosting the message.
        /// </param>
        /// <param name='criteria'>
        /// The search criteria to use in order to find a match.
        /// </param>
        public async Task<Message> WaitForAsync(string server, SearchCriteria criteria)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, "api/messages/await?server=" + server))
            {
                var requestContent = JsonConvert.SerializeObject(criteria);
                request.Content = new StringContent(requestContent, Encoding.UTF8, "application/json");
                using (var response = await _client.SendAsync(request))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        await ThrowExceptionAsync(response);

                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Message>(content);
                }
            }
        }
    }
}
