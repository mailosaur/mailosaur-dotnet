namespace Mailosaur.Models
{
    using System.Collections.Generic;

    public class UsageTransactionListResult
    {
        /// <summary>
        /// Gets or sets the individual transactions forming the result.
        /// </summary>
        public IList<UsageTransaction> Items { get; set; }

    }
}
