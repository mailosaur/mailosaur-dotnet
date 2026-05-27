namespace Mailosaur.Models
{
    using System.Collections.Generic;

    public class Server
    {
        /// <summary>
        /// Gets or sets unique identifier for the inbox (server). Used as username for
        /// SMTP/POP3 authentication.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets a name used to identify the inbox (server).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets users (excluding administrators) who have access to
        /// the inbox (server) when access is restricted.
        /// </summary>
        public IList<string> Users { get; set; }

        /// <summary>
        /// Gets or sets the number of messages currently in the inbox (server).
        /// </summary>
        public int? Messages { get; set; }

    }
}
