using System.Collections.Generic;

namespace Mailosaur.Models
{
    public class MessageCreateOptions
    {
        /// <summary>
        /// Initializes a new instance of the MessageCreateOptions class.
        /// </summary>
        public MessageCreateOptions()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MessageCreateOptions class.
        /// </summary>
        /// <param name="to">The email address to which the email will be sent.</param>
        /// <param name="from">Partially overrides of the message's 'from' address. This **must** be an address ending 
        /// with `YOUR_SERVER.mailosaur.net`, such as `my-emails @a1bcdef2.mailosaur.net`.</param>
        /// <param name="send">If true, email will be sent upon creation.</param>
        /// <param name="subject">The email subject line.</param>
        /// <param name="text">The plain text body of the email. Note that only text or html can be supplied, not both.</param>
        /// <param name="html">The HTML body of the email. Note that only text or html can be supplied, not both.</param>
        /// <param name="att">The email subject line.</param>
        public MessageCreateOptions(string to, bool send, string subject, string text = null, string html = null, IEnumerable<Attachment> attachments = null)
        {
            To = to;
            Send = send;
            Subject = subject;
            Text = text;
            Html = html;
            Attachments = attachments;
        }

        /// <summary>
        /// The email address to which the email will be sent.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Partially overrides of the message's 'from' address. This **must** be an address ending 
        /// with `YOUR_SERVER.mailosaur.net`, such as `my-emails @a1bcdef2.mailosaur.net`.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// If true, email will be sent upon creation.
        /// </summary>
        public bool Send { get; set; }

        /// <summary>
        /// The email subject line.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The plain text body of the email. Note that only text or html can be supplied, not both.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The HTML body of the email. Note that only text or html can be supplied, not both.
        /// </summary>
        public string Html { get; set; }

        /// <summary>
        /// Any message attachments.
        /// </summary>
        public IEnumerable<Attachment> Attachments { get; set; }
    }
}
