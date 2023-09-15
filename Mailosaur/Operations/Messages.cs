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
        public Message Get(string server, SearchCriteria criteria = null, int timeout = 10000, DateTime? receivedAfter = null)
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
        public async Task<Message> GetAsync(string server, SearchCriteria criteria = null, int timeout = 10000, DateTime? receivedAfter = null)
        {
            // Timeout defaulted to 10s, receivedAfter to 1h
            receivedAfter = receivedAfter != null ? receivedAfter : DateTime.UtcNow.AddHours(-1);
            criteria = criteria != null ? criteria : new SearchCriteria();

            if (server.Length != 8)
                throw new MailosaurException("Must provide a valid Server ID.", "invalid_request");

            var result = await SearchAsync(server, criteria, 0, 1, timeout, receivedAfter);
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
        /// <param name='receivedAfter'>
        /// Limits results to only messages received after this date/time.
        /// </param>
        /// <param name='dir'>
        /// Optionally limits results based on the direction (`Sent` or `Received`),
        /// with the default being `Received`.
        /// </param>
        public MessageListResult List(string server, int? page = default(int?), int? itemsPerPage = default(int?), DateTime? receivedAfter = null, string dir = null)
            => Task.Run(async () => await ListAsync(server, page, itemsPerPage, receivedAfter, dir)).UnwrapException<MessageListResult>();

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
        /// <param name='receivedAfter'>
        /// Limits results to only messages received after this date/time.
        /// </param>
        /// <param name='dir'>
        /// Optionally limits results based on the direction (`Sent` or `Received`),
        /// with the default being `Received`.
        /// </param>
        public Task<MessageListResult> ListAsync(string server, int? page = default(int?), int? itemsPerPage = default(int?), DateTime? receivedAfter = null, string dir = null)
            => ExecuteRequest<MessageListResult>(HttpMethod.Get, PagePath($"api/messages?server={server}", page, itemsPerPage, receivedAfter, dir));

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
        /// <param name='errorOnTimeout'>
        ///  When set to false, an error will not be throw if timeout is reached
        ///  (default: true).
        /// </param>
        /// <param name='dir'>
        /// Optionally limits results based on the direction (`Sent` or `Received`),
        /// with the default being `Received`.
        /// </param>
        public MessageListResult Search(string server, SearchCriteria criteria, int? page = null, int? itemsPerPage = null, int? timeout = null, DateTime? receivedAfter = null, bool errorOnTimeout = true, string dir = null)
            => Task.Run(async () => await SearchAsync(server, criteria, page, itemsPerPage, timeout, receivedAfter, errorOnTimeout, dir)).UnwrapException<MessageListResult>();

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
        /// <param name='errorOnTimeout'>
        ///  When set to false, an error will not be throw if timeout is reached
        ///  (default: true).
        /// </param>
        /// <param name='dir'>
        /// Optionally limits results based on the direction (`Sent` or `Received`),
        /// with the default being `Received`.
        /// </param>
        public async Task<MessageListResult> SearchAsync(string server, SearchCriteria criteria, int? page = null, int? itemsPerPage = null, int? timeout = null, DateTime? receivedAfter = null, bool errorOnTimeout = true, string dir = null)
        {
            var pollCount = 0;
            var startTime = DateTime.UtcNow;

            while (true)
            {
                var result = await ExecuteRequest<MessageListResultWithHeaders>(HttpMethod.Post, PagePath($"api/messages/search?server={server}", page, itemsPerPage, receivedAfter, dir), criteria);

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
                {
                    if (errorOnTimeout == false)
                    {
                        return result.MessageListResult;
                    }

                    throw new MailosaurException("No matching messages found in time. By default, only messages received in the last hour are checked (use receivedAfter to override this).", "search_timeout");
                }

                Task.Delay(delay).Wait();
            }
        }

        /// <summary>
        /// Create a message
        /// </summary>
        /// <remarks>
        /// Creates a new message that can be sent to a verified email address. This is 
        /// useful in scenarios where you want an email to trigger a workflow in your
        /// product.
        /// </remarks>
        /// <param name='server'>
        /// The identifier of the server to create the message in.
        /// </param>
        /// <param name='messageCreateOptions'>
        /// The options with which to create the message.
        /// </param>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public Message Create(string server, MessageCreateOptions messageCreateOptions)
            => Task.Run(async () => await CreateAsync(server, messageCreateOptions)).UnwrapException<Message>();

        /// <summary>
        /// Create a message
        /// </summary>
        /// <remarks>
        /// Creates a new message that can be sent to a verified email address. This is 
        /// useful in scenarios where you want an email to trigger a workflow in your
        /// product.
        /// </remarks>
        /// <param name='server'>
        /// The identifier of the server to create the message in.
        /// </param>
        /// <param name='messageCreateOptions'>
        /// The options with which to create the message.
        /// </param>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public Task<Message> CreateAsync(string server, MessageCreateOptions messageCreateOptions)
            => ExecuteRequest<Message>(HttpMethod.Post, $"api/messages?server={server}", messageCreateOptions);

        /// <summary>
        /// Forward an email
        /// </summary>
        /// <remarks>
        /// Forwards the specified email to a verified email address.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the email to forward.
        /// </param>
        /// <param name='messageForwardOptions'>
        /// The options with which to forward the email.
        /// </param>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public Message Forward(string id, MessageForwardOptions messageForwardOptions)
            => Task.Run(async () => await ForwardAsync(id, messageForwardOptions)).UnwrapException<Message>();

        /// <summary>
        /// Forward an email
        /// </summary>
        /// <remarks>
        /// Forwards the specified email to a verified email address.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the email to forward.
        /// </param>
        /// <param name='messageForwardOptions'>
        /// The options with which to forward the email.
        /// </param>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public Task<Message> ForwardAsync(string id, MessageForwardOptions messageForwardOptions)
            => ExecuteRequest<Message>(HttpMethod.Post, $"api/messages/{id}/forward", messageForwardOptions);

        /// <summary>
        /// Reply to an email
        /// </summary>
        /// <remarks>
        /// Sends a reply to the specified email. This is useful for when simulating a user
        /// replying to one of your emails.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the email to reply to.
        /// </param>
        /// <param name='messageReplyOptions'>
        /// The options with which to reply to the email.
        /// </param>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public Message Reply(string id, MessageReplyOptions messageReplyOptions)
            => Task.Run(async () => await ReplyAsync(id, messageReplyOptions)).UnwrapException<Message>();

        /// <summary>
        /// Reply to an email
        /// </summary>
        /// <remarks>
        /// Sends a reply to the specified email. This is useful for when simulating a user
        /// replying to one of your emails.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the email to reply to.
        /// </param>
        /// <param name='messageReplyOptions'>
        /// The options with which to reply to the email.
        /// </param>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public Task<Message> ReplyAsync(string id, MessageReplyOptions messageReplyOptions)
            => ExecuteRequest<Message>(HttpMethod.Post, $"api/messages/{id}/reply", messageReplyOptions);

        /// <summary>
        /// Generate email previews
        /// </summary>
        /// <remarks>
        /// Generates screenshots of an email rendered in the specified email clients.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the email to preview.
        /// </param>
        /// <param name='options'>
        /// The options with which to generate previews.
        /// </param>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public PreviewListResult GeneratePreviews(string id, PreviewRequestOptions options)
            => Task.Run(async () => await GeneratePreviewsAsync(id, options)).UnwrapException<PreviewListResult>();

        /// <summary>
        /// Generate email previews
        /// </summary>
        /// <remarks>
        /// Generates screenshots of an email rendered in the specified email clients.
        /// </remarks>
        /// <param name='id'>
        /// The identifier of the email to preview.
        /// </param>
        /// <param name='options'>
        /// The options with which to generate previews.
        /// </param>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public Task<PreviewListResult> GeneratePreviewsAsync(string id, PreviewRequestOptions options)
            => ExecuteRequest<PreviewListResult>(HttpMethod.Post, $"api/messages/{id}/previews", options);
    }
}
