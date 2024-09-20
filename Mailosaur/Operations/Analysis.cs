namespace Mailosaur.Operations
{
    using Models;
    using System.Net.Http;
    using System.Threading.Tasks;

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
        public Task<DeliverabilityReport> DeliverabilityAsync(string email)
            => ExecuteRequest<DeliverabilityReport>(HttpMethod.Get, $"api/analysis/deliverability/{email}");
    }
}
