namespace Mailosaur.Models
{
    using System.Collections.Generic;

    public class Metadata
    {
        /// <summary>
        /// Gets or sets email headers.
        /// </summary>
        public IList<MessageHeader> Headers { get; set; }

        /// <summary>
        /// TBC
        /// </summary>
        public string Ehlo { get; set; }

        /// <summary>
        /// TBC
        /// </summary>
        public string MailFrom { get; set; }

        /// <summary>
        /// Gets or sets the rcpt value of the message.
        /// </summary>
        public IList<MessageAddress> RcptTo { get; set; }
    }
}
