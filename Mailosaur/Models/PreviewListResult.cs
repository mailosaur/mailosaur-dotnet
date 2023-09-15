namespace Mailosaur.Models
{
    using System.Collections.Generic;

    public class PreviewListResult
    {
        /// <summary>
        /// A list of requested email previews.
        /// </summary>
        public IList<Preview> Items { get; set; }

    }
}
