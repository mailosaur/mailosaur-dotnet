namespace Mailosaur.Models
{
    public class Preview
    {
        /// <summary>
        /// The unique identifier for the email preview.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The email client the preview was generated with.
        /// </summary>
        public string EmailClient { get; set; }

        /// <summary>
        /// Whether images were disabled in the preview.
        /// </summary>
        public bool DisableImages { get; set; }

    }
}
