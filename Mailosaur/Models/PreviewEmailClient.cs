namespace Mailosaur.Models
{
    public class PreviewEmailClient
    {
        /// <summary>
        /// The unique identifier for the email preview.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The display name of the email client.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Whether the platform is desktop, mobile, or web-based.
        /// </summary>
        public string PlatformGroup { get; set; }

        /// <summary>
        /// The type of platform on which the email client is running.
        /// </summary>
        public string PlatformType { get; set; }

        /// <summary>
        /// The platform version number.
        /// </summary>
        public string PlatformVersion { get; set; }

        /// <summary>
        /// Whether images can be disabled when generating previews.
        /// </summary>
        public bool CanDisableImages { get; set; }

        /// <summary>
        /// The current status of the email client.
        /// </summary>
        public string Status { get; set; }

    }
}
