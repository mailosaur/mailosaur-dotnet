namespace Mailosaur.Operations
{
    using System;
    using System.Linq;
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
        /// Retrieve a message using search criteria
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
        /// <param name='timeout'>
        /// Specify how long to wait for a matching result (in milliseconds).
        /// </param>
        /// <param name='receivedAfter'>
        /// Limits results to only messages received after this date/time.
        /// </param>
        public Message Get(string server, SearchCriteria criteria, int timeout = 10000, DateTime? receivedAfter = null)
            => Task.Run(async () => await GetAsync(server, criteria, timeout, receivedAfter)).UnwrapException<Message>();
        
        /// <summary>
        /// Retrieve a message using search criteria
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
        /// <param name='timeout'>
        /// Specify how long to wait for a matching result (in milliseconds).
        /// </param>
        /// <param name='receivedAfter'>
        /// Limits results to only messages received after this date/time.
        /// </param>
        public async Task<Message> GetAsync(string server, SearchCriteria criteria, int timeout = 10000, DateTime? receivedAfter = null)
        {
            // Timeout defaulted to 10s, receivedAfter to 1h
            receivedAfter = receivedAfter != null ? receivedAfter : DateTime.UtcNow.AddHours(-1);

            if (server.Length > 8)
                throw new Exception("Use GetById to retrieve a message using its identifier");
            
            var result = await SearchAsync(server, criteria, timeout: timeout, receivedAfter: receivedAfter);
            return GetById(result.Items[0].Id);
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
        public Message GetById(string id)
            => Task.Run(async () => await GetByIdAsync(id)).UnwrapException<Message>();

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
        public Task<Message> GetByIdAsync(string id)
            => ExecuteRequest<Message>(HttpMethod.Get, $"api/messages/{id}");

        /// <summary>
        ///  a message
        /// </summary>
        /// <remarks>
        /// Permanently deletes a message. This operation cannot be undone. Also
        /// deletes any attachments related to the message.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the message to be deleted.
        /// </param>
        public void Delete(string id)
            => Task.Run(async () => await DeleteAsync(id)).WaitAndUnwrapException();

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
            => Task.Run(async () => await ListAsync(server, page, itemsPerPage)).UnwrapException<MessageListResult>();

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
            => Task.Run(async () => await DeleteAllAsync(server)).WaitAndUnwrapException();

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
        /// <param name='timeout'>
        /// Specify how long to wait for a matching result (in milliseconds).
        /// </param>
        /// <param name='receivedAfter'>
        /// Limits results to only messages received after this date/time.
        /// </param>
        public MessageListResult Search(string server, SearchCriteria criteria, int? page = null, int? itemsPerPage = null, int? timeout = null, DateTime? receivedAfter = null)
            => Task.Run(async () => await SearchAsync(server, criteria, page, itemsPerPage, timeout, receivedAfter)).UnwrapException<MessageListResult>();

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
        /// <param name='timeout'>
        /// Specify how long to wait for a matching result (in milliseconds).
        /// </param>
        /// <param name='receivedAfter'>
        /// Limits results to only messages received after this date/time.
        /// </param>
        public async Task<MessageListResult> SearchAsync(string server, SearchCriteria criteria, int? page = null, int? itemsPerPage = null, int? timeout = null, DateTime? receivedAfter = null)
        {
            var pollCount = 0;
            var startTime = DateTime.UtcNow;

            while(true)
            {
                var result = await ExecuteRequest<MessageListResultWithHeaders>(HttpMethod.Post, PagePath($"api/messages/search?server={server}", page, itemsPerPage, receivedAfter), criteria);

                if (timeout == null || timeout == 0 || result.MessageListResult.Items.Count != 0)
                    return result.MessageListResult;
                
                var delayPattern = (string.IsNullOrWhiteSpace(result.DelayHeader) ? "1000" : result.DelayHeader)
                    .Split(',').Select(x => Int32.Parse(x)).ToArray();
                
                var delay = pollCount >= delayPattern.Length ?
                    delayPattern[delayPattern.Length - 1] :
                    delayPattern[pollCount];

                pollCount++;

                // Stop if timeout will be exceeded
                if (((int)(DateTime.UtcNow - startTime).TotalMilliseconds) + delay > timeout)
                    throw new Exception("No matching messages were found in time");
                
                Task.Delay(delay).Wait();
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
            => Task.Run(async () => await WaitForAsync(server, criteria)).UnwrapException<Message>();

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
