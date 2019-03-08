namespace Mailosaur.Models
{
    public class ServerCreateOptions
    {
        /// <summary>
        /// Initializes a new instance of the ServerCreateOptions class.
        /// </summary>
        /// <param name="name">A name used to identify the server.</param>
        public ServerCreateOptions(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets or sets a name used to identify the server.
        /// </summary>
        public string Name { get; set; }

    }
}
