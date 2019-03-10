namespace Mailosaur.Operations
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Mailosaur.Models;

    public class Messages : OperationBase
    {
        /// <summary>
        /// Initializes a new instance of the Messages class.
        /// </summary>
        /// <param name='client'>
        /// Reference to the HttpClient.
        /// </param>
        public Messages(HttpClient client) : base(client) { }

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
            => HandleAggregateException<Message>(() => GetAsync(id).Result);

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
        public Task<Message> GetAsync(string id)
            => ExecuteRequest<Message>(HttpMethod.Get, $"api/messages/{id}");

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
            => HandleAggregateException(() => DeleteAsync(id).Wait());

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
        public Task DeleteAsync(string id)
            => ExecuteRequest(HttpMethod.Delete, $"api/messages/{id}");

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
            => HandleAggregateException<MessageListResult>(() => ListAsync(server, page, itemsPerPage).Result);

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
        public Task<MessageListResult> ListAsync(string server, int? page = default(int?), int? itemsPerPage = default(int?))
            => ExecuteRequest<MessageListResult>(HttpMethod.Get, PagePath($"api/messages?server={server}", page, itemsPerPage));

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
            => HandleAggregateException(() => DeleteAllAsync(server).Wait());

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
        public Task DeleteAllAsync(string server) 
            => ExecuteRequest(HttpMethod.Delete, $"api/messages?server={server}");

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
            => HandleAggregateException<MessageListResult>(() => SearchAsync(server, criteria, page, itemsPerPage).Result);

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
        public Task<MessageListResult> SearchAsync(string server, SearchCriteria criteria, int? page = null, int? itemsPerPage = null)
            => ExecuteRequest<MessageListResult>(HttpMethod.Post, PagePath($"api/messages/search?server={server}", page, itemsPerPage), criteria);

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
            => HandleAggregateException<Message>(() => WaitForAsync(server, criteria).Result);

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
        public Task<Message> WaitForAsync(string server, SearchCriteria criteria)
            => ExecuteRequest<Message>(HttpMethod.Post, $"api/messages/await?server={server}", criteria);
    }
}
