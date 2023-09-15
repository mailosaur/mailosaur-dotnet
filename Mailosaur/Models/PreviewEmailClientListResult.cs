namespace Mailosaur.Models
{
    using System.Collections.Generic;

    public class PreviewEmailClientListResult
    {
        /// <summary>
        /// A list of available email clients with which to generate email previews.
        /// </summary>
        public IList<PreviewEmailClient> Items { get; set; }

    }
}
