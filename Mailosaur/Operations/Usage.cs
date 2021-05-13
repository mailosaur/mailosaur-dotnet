namespace Mailosaur.Operations
{
    using Models;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System;

    /// <summary>
    /// Usage operations.
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
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public UsageAccountLimits Limits()
            => Task.Run(async () => await LimitsAsync()).UnwrapException<UsageAccountLimits>();

        /// <summary>
        /// Retrieve account usage limits.
        /// </summary>
        /// <remarks>
        /// Details the current limits and usage for your account.
        /// </remarks>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public Task<UsageAccountLimits> LimitsAsync()
            => ExecuteRequest<UsageAccountLimits>(HttpMethod.Get, $"api/usage/limits");

        /// <summary>
        /// List account transactions.
        /// </summary>
        /// <remarks>
        /// Retrieves the last 31 days of transactional usage.
        /// </remarks>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public UsageTransactionListResult Transactions()
            => Task.Run(async () => await TransactionsAsync()).UnwrapException<UsageTransactionListResult>();

        /// <summary>
        /// List account transactions.
        /// </summary>
        /// <remarks>
        /// Retrieves the last 31 days of transactional usage.
        /// </remarks>
        /// <exception cref="MailosaurException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public Task<UsageTransactionListResult> TransactionsAsync()
            => ExecuteRequest<UsageTransactionListResult>(HttpMethod.Get, $"api/usage/transactions");
    }
}
