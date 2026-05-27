namespace Mailosaur.Operations
{
    using Models;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Operations for analyzing the content and deliverability of an email, including SpamAssassin
    /// scoring and per-provider deliverability reports. Accessed via <c>client.Analysis</c>.
    /// </summary>
    public class Analysis : OperationBase
    {
        /// <summary>
        /// Initializes a new instance of the Analysis class.
        /// </summary>
        /// <param name='client'>
        /// Reference to the HttpClient.
        /// </param>
        public Analysis(HttpClient client) : base(client) { }

        /// <summary>
        /// Perform a spam test
        /// </summary>
        /// <remarks>
        /// Perform spam testing on the specified email
        /// </remarks>
        /// <param name='email'>
        /// The identifier of the email to be analyzed.
        /// </param>
        /// <returns>
        /// A <see cref="SpamAnalysisResult"/> containing the spam score and filter results.
        /// </returns>
        public SpamAnalysisResult Spam(string email)
            => Task.Run<SpamAnalysisResult>(async () => await SpamAsync(email)).Result;

        /// <summary>
        /// Perform a spam test
        /// </summary>
        /// <remarks>
        /// Perform spam testing on the specified email
        /// </remarks>
        /// <param name='email'>
        /// The identifier of the email to be analyzed.
        /// </param>
        /// <returns>
        /// A task resolving to a <see cref="SpamAnalysisResult"/> containing the spam score and filter results.
        /// </returns>
        public Task<SpamAnalysisResult> SpamAsync(string email)
            => ExecuteRequest<SpamAnalysisResult>(HttpMethod.Get, $"api/analysis/spam/{email}");
        
        /// <summary>
        /// Perform a deliverability test 
        /// </summary>
        /// <remarks>
        /// Perform deliverability testing on the specified email
        /// </remarks>
        /// <param name='email'>
        /// The identifier of the email to be analyzed.
        /// </param>
        /// <returns>
        /// A <see cref="DeliverabilityReport"/> for the email.
        /// </returns>
        public DeliverabilityReport Deliverability(string email)
            => Task.Run<DeliverabilityReport>(async () => await DeliverabilityAsync(email)).Result;
        
        /// <summary>
        /// Perform a deliverability test
        /// </summary>
        /// <remarks>
        /// Perform deliverability testing on the specified email
        /// </remarks>
        /// <param name='email'>
        /// The identifier of the email to be analyzed.
        /// </param>
        /// <returns>
        /// A task resolving to a <see cref="DeliverabilityReport"/> for the email.
        /// </returns>
        public Task<DeliverabilityReport> DeliverabilityAsync(string email)
            => ExecuteRequest<DeliverabilityReport>(HttpMethod.Get, $"api/analysis/deliverability/{email}");
    }
}
