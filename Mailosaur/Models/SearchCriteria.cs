namespace Mailosaur.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public class SearchCriteria
    {
        /// <summary>
        /// Initializes a new instance of the SearchCriteria class.
        /// </summary>
        /// <param name="sentFrom">The full email address from which the target
        /// email was sent.</param>
        /// <param name="sentTo">The full email address to which the target
        /// email was sent.</param>
        /// <param name="subject">The value to seek within the target email's
        /// subject line.</param>
        /// <param name="body">The value to seek within the target email's HTML
        /// or text body.</param>
        /// <param name="match">If set to ALL (default), then only results that match all 
        /// specified criteria will be returned. If set to ANY, results that match any of the
        /// specified criteria will be returned.</param>
        public SearchCriteria(string sentFrom = null, string sentTo = null, string subject = null, string body = null, SearchMatchOperator match = SearchMatchOperator.ALL)
        {
            SentFrom = sentFrom;
            SentTo = sentTo;
            Subject = subject;
            Body = body;
            Match = match;
        }

        /// <summary>
        /// Gets or sets the full email address from which the target email was
        /// sent.
        /// </summary>
        public string SentFrom { get; set; }
        
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

        /// <summary>
        /// If set to ALL (default), then only results that match all 
        /// specified criteria will be returned. If set to ANY, results that match any of the
        /// specified criteria will be returned.
        /// </summary>
        public SearchMatchOperator Match { get; set; }
    }
}
