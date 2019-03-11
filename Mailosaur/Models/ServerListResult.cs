namespace Mailosaur.Models
{
    using System.Collections.Generic;

    public class ServerListResult
    {
        /// <summary>
        /// Gets or sets the individual servers forming the result. Servers are
        /// returned sorted by creation date, with the most recently-created
        /// server appearing first.
        /// </summary>
        public IList<Server> Items { get; set; }

    }
}
