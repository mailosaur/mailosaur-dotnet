using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mailosaur.Models
{
    public class PreviewRequestOptions
    {
        /// <summary>
        /// Sets the list email clients to generate previews with.
        /// </summary>
        public PreviewRequestOptions(IEnumerable<string> emailClients)
        {
            EmailClients = emailClients;
        }

        /// <summary>
        /// The list email clients to generate previews with.
        /// </summary>
        [JsonProperty("emailClients")]
        public IEnumerable<string> EmailClients { get; set; }
    }
}
