namespace Mailosaur.Models
{
    public class PreviewRequest
    {
        public PreviewRequest(string emailClient, bool disableImages = false)
        {
            EmailClient = emailClient;
            DisableImages = disableImages;
        }

        /// <summary>
        /// Gets or sets the email client you wish to generate a preview for.
        /// </summary>
        public string EmailClient { get; set; }

        /// <summary>
        /// Gets or sets whether images will be disabled (only if supported by the client).
        /// </summary>
        public bool DisableImages { get; set; }
    }
}
