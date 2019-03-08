namespace Mailosaur.Models
{
    public class MessageAddress
    {
        /// <summary>
        /// Gets or sets display name, if one is specified.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets email address (applicable to email messages).
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets phone number (applicable to SMS messages).
        /// </summary>
        public string Phone { get; set; }

    }
}
