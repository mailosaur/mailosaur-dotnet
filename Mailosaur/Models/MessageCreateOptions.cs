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
        /// <param name="send">If true, email will be sent upon creation.</param>
        /// <param name="subject">The email subject line.</param>
        /// <param name="text">The plain text body of the email. Note that only text or html can be supplied, not both.</param>
        /// <param name="html">The HTML body of the email. Note that only text or html can be supplied, not both.</param>
        public MessageCreateOptions(string to, bool send, string subject, string text = null, string html = null)
        {
            To = to;
            Send = send;
            Subject = subject;
            Text = text;
            Html = html;
        }

        /// <summary>
        /// The email address to which the email will be sent.
        /// </summary>
        public string To { get; set; }
        
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
    }
}
