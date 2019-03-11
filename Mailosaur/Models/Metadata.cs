namespace Mailosaur.Models
{
    using System.Collections.Generic;

    public class Metadata
    {
        /// <summary>
        /// Gets or sets email headers.
        /// </summary>
        public IList<MessageHeader> Headers { get; set; }

    }
}
