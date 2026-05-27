namespace Mailosaur.Operations
{
    using Models;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System;

    /// <summary>
    /// Operations for inspecting your account's usage limits and recent transactional usage.
    /// These endpoints require authentication with an account-level API key. Accessed via <c>client.Usage</c>.
    /// </summary>
    public class Usage : OperationBase
    {
        /// <summary>
        /// Initializes a new instance of the Usage class.
        /// </summary>
        /// <param name='client'>
        /// Reference to the HttpClient.
        /// </param>
        public Usage(HttpClient client) : base(client) { }

        /// <summary>
        /// Retrieve account usage limits.
        /// </summary>
        /// <remarks>
        /// Details the current limits and usage for your account.
        /// </remarks>
        /// <returns>
        /// The <see cref="UsageAccountLimits"/> for your account.
        /// </returns>
        public UsageAccountLimits Limits()
            => Task.Run(async () => await LimitsAsync()).UnwrapException<UsageAccountLimits>();

        /// <summary>
        /// Retrieve account usage limits.
        /// </summary>
        /// <remarks>
        /// Details the current limits and usage for your account.
        /// </remarks>
        /// <returns>
        /// A task resolving to the <see cref="UsageAccountLimits"/> for your account.
        /// </returns>
        public Task<UsageAccountLimits> LimitsAsync()
            => ExecuteRequest<UsageAccountLimits>(HttpMethod.Get, $"api/usage/limits");

        /// <summary>
        /// List account transactions.
        /// </summary>
        /// <remarks>
        /// Retrieves the last 31 days of transactional usage.
        /// </remarks>
        /// <returns>
        /// A <see cref="UsageTransactionListResult"/> for the last 31 days.
        /// </returns>
        public UsageTransactionListResult Transactions()
            => Task.Run(async () => await TransactionsAsync()).UnwrapException<UsageTransactionListResult>();

        /// <summary>
        /// List account transactions.
        /// </summary>
        /// <remarks>
        /// Retrieves the last 31 days of transactional usage.
        /// </remarks>
        /// <returns>
        /// A task resolving to a <see cref="UsageTransactionListResult"/> for the last 31 days.
        /// </returns>
        public Task<UsageTransactionListResult> TransactionsAsync()
            => ExecuteRequest<UsageTransactionListResult>(HttpMethod.Get, $"api/usage/transactions");
    }
}
