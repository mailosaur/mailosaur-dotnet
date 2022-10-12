namespace Mailosaur.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class MessageSummary
    {
        /// <summary>
        /// Gets or sets unique identifier for the message.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the type of message.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets identifier for the server in which the message is
        /// located.
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the sender of the message.
        /// </summary>
        public IList<MessageAddress> From { get; set; }

        /// <summary>
        /// Gets or sets the message’s recipient.
        /// </summary>
        public IList<MessageAddress> To { get; set; }

        /// <summary>
        /// Gets or sets carbon-copied recipients for email messages.
        /// </summary>
        public IList<MessageAddress> Cc { get; set; }

        /// <summary>
        /// Gets or sets blind carbon-copied recipients for email messages.
        /// </summary>
        public IList<MessageAddress> Bcc { get; set; }

        /// <summary>
        /// Gets or sets the datetime that this message was received by
        /// Mailosaur.
        /// </summary>
        public System.DateTime? Received { get; set; }

        /// <summary>
        /// Gets or sets the message’s subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Summary snippet taken from message body
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Number of message attachments 
        /// </summary>
        public int? Attachments { get; set; }
    }
}
