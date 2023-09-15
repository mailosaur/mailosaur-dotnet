using System.Collections.Generic;

namespace Mailosaur.Models
{
    public class PreviewRequestOptions
    {
        /// <summary>
        /// Sets the list of email preview requests.
        /// </summary>
        public PreviewRequestOptions(IEnumerable<PreviewRequest> previews)
        {
            Previews = previews;
        }

        /// <summary>
        /// The list of email preview requests.
        /// </summary>
        public IEnumerable<PreviewRequest> Previews { get; set; }
    }
}
