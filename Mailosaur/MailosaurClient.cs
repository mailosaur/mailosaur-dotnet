using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Mailosaur.Operations;

namespace Mailosaur
{

    public class MailosaurClient
    {
        public Servers Servers { get; }
        public Messages Messages { get; }
        public Files Files { get; }
        public Analysis Analysis { get; }

        /// <summary>
        /// Initializes a new instance of the MailosaurClient class.
        /// </summary>
        /// <param name='apiKey'>
        /// Your Mailosaur API key.
        /// </param>
        /// <param name='baseUrl'>
        /// Optional. Override the base URL for the mailosaur server.
        /// </param>
        public MailosaurClient(string apiKey)
        {
            var httpClient = new HttpClient {BaseAddress = new Uri("https://mailosaur.com/")};
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "mailosaur-dotnet/5.0.0");
            
            var apiKeyBytes = Encoding.ASCII.GetBytes($"{apiKey}:");
            var apiKeyBase64 = Convert.ToBase64String(apiKeyBytes);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", apiKeyBase64);

            Servers = new Servers(httpClient);
            Messages = new Messages(httpClient);
            Files = new Files(httpClient);
            Analysis = new Analysis(httpClient);
        }
    }
}
