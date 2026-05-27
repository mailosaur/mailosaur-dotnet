namespace Mailosaur.Models
{
    using System.Collections.Generic;

    public class ServerListResult
    {
        /// <summary>
        /// Gets or sets the individual inboxes (servers) forming the result. Inboxes
        /// (servers) are returned sorted by creation date, with the most recently-created
        /// inbox (server) appearing first.
        /// </summary>
        public IList<Server> Items { get; set; }

    }
}
