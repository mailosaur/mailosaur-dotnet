using System.Collections.Generic;

namespace Mailosaur.Models
{
    public class MessageReplyOptions
    {
        /// <summary>
        /// Initializes a new instance of the MessageReplyOptions class.
        /// </summary>
        public MessageReplyOptions()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MessageReplyOptions class.
        /// </summary>
        /// <param name="text">Any additional plain text content to include in the reply. Note that only text or html can be supplied, not both.</param>
        /// <param name="html">Any additional HTML content to include in the reply. Note that only html or text can be supplied, not both.</param>
        public MessageReplyOptions(string text = null, string html = null, IEnumerable<Attachment> attachments = null)
        {
            Text = text;
            Html = html;
            Attachments = attachments;
        }

        /// <summary>
        /// Any additional plain text content to include in the reply. Note that only text or html can be supplied, not both.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Any additional HTML content to include in the reply. Note that only html or text can be supplied, not both.
        /// </summary>
        public string Html { get; set; }

        /// <summary>
        /// Any message attachments.
        /// </summary>
        public IEnumerable<Attachment> Attachments { get; set; }
    }
}
