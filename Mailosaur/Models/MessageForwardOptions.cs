namespace Mailosaur.Models
{
    public class MessageForwardOptions
    {
        /// <summary>
        /// Initializes a new instance of the MessageForwardOptions class.
        /// </summary>
        public MessageForwardOptions()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MessageForwardOptions class.
        /// </summary>
        /// <param name="to">The email address to which the email will be sent.</param>
        /// <param name="cc">The email address to which the email will be CC'd.</param>
        /// <param name="text">Any additional plain text content to forward the email with. Note that only text or html can be supplied, not both.</param>
        /// <param name="html">Any additional HTML content to forward the email with. Note that only html or text can be supplied, not both.</param>
        public MessageForwardOptions(string to, string text = null, string html = null, string cc = null)
        {
            To = to;
            Text = text;
            Html = html;
        }

        /// <summary>
        /// The email address to which the email will be sent.
        /// </summary>
        public string To { get; set; }
        
        /// <summary>
        /// The email address to which the email will be CC'd.
        /// </summary>
        public string Cc { get; set; }
        
        /// <summary>
        /// Any additional plain text content to forward the email with. Note that only text or html can be supplied, not both.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Any additional HTML content to forward the email with. Note that only html or text can be supplied, not both.
        /// </summary>
        public string Html { get; set; }
    }
}
