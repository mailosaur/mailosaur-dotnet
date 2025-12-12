namespace Mailosaur.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// A list of available email clients with which to generate email previews.
    /// </summary>
    public class EmailClientListResult
    {
        /// <summary>
        /// A list of available email clients with which to generate email previews.
        /// </summary>
        [JsonProperty("items")]
        public IList<EmailClient> Items { get; set; }
    }
}
