namespace Mailosaur.Models
{
    public class UsageTransaction
    {
        /// <summary>
        /// Gets or sets the datetime that this transaction occurred.
        /// </summary>
        public System.DateTime Timestamp { get; set; }

        /// <summary>
        /// The count of email transactions.
        /// </summary>
        public int Email { get; set; }

        /// <summary>
        /// The count of SMS transactions.
        /// </summary>
        public int Sms { get; set; }
    }
}
