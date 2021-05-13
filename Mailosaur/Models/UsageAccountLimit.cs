namespace Mailosaur.Models
{
    public class UsageAccountLimit
    {
        /// <summary>
        /// The limit
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// The current value
        /// </summary>
        public int Current { get; set; }
    }
}
