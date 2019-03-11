namespace Mailosaur.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public class SearchCriteria
    {
        /// <summary>
        /// Initializes a new instance of the SearchCriteria class.
        /// </summary>
        /// <param name="sentTo">The full email address to which the target
        /// email was sent.</param>
        /// <param name="subject">The value to seek within the target email's
        /// subject line.</param>
        /// <param name="body">The value to seek within the target email's HTML
        /// or text body.</param>
        public SearchCriteria(string sentTo = null, string subject = null, string body = null)
        {
            SentTo = sentTo;
            Subject = subject;
            Body = body;
        }

        /// <summary>
        /// Gets or sets the full email address to which the target email was
        /// sent.
        /// </summary>
        public string SentTo { get; set; }

        /// <summary>
        /// Gets or sets the value to seek within the target email's subject
        /// line.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the value to seek within the target email's HTML or
        /// text body.
        /// </summary>
        public string Body { get; set; }

    }
}
