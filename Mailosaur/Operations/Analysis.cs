namespace Mailosaur.Operations
{
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class Analysis : OperationBase
    {
        /// <summary>
        /// Initializes a new instance of the Analysis class.
        /// </summary>
        /// <param name='client'>
        /// Reference to the http client.
        /// </param>
        public Analysis(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Gets a reference to the HttpClient
        /// </summary>
        private readonly HttpClient _client;

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
        {
            try
            {
                return SpamAsync(email).Result;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Perform a spam test
        /// </summary>
        /// <remarks>
        /// Perform spam testing on the specified email
        /// </remarks>
        /// <param name='email'>
        /// The identifier of the email to be analyzed.
        /// </param>
        public async Task<SpamAnalysisResult> SpamAsync(string email)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, "api/analysis/spam/" + email))
            using (var response = await _client.SendAsync(request))
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    await ThrowExceptionAsync(response);

                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SpamAnalysisResult>(content);
            }
        }
    }
}
