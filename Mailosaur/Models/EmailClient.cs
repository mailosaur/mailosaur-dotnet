namespace Mailosaur.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Describes an email client with which email previews can be generated.
    /// </summary>
    public class EmailClient
    {
        /// <summary>
        /// The unique email client label. Used when generating email preview requests.
        /// </summary>
        [JsonProperty("label")]
        public string Label { get; set; }

        /// <summary>
        /// The display name of the email client.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
