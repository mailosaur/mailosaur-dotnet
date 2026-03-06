namespace Mailosaur
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using Mailosaur.Models;
    using Mailosaur.Operations;

    public class MailosaurClient : IDisposable
    {
        public Servers Servers { get; private set; }
        public Messages Messages { get; private set; }
        public Files Files { get; private set; }
        public Analysis Analysis { get; private set; }
        public Usage Usage { get; private set; }
        public Devices Devices { get; private set; }
        public Previews Previews { get; private set; }

        private readonly HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the MailosaurClient class.
        /// If no API key is provided, the MAILOSAUR_API_KEY environment variable is used.
        /// </summary>
        /// <param name='apiKey'>
        /// Optional. Your Mailosaur API key. If not provided, the MAILOSAUR_API_KEY
        /// environment variable will be used.
        /// </param>
        /// <param name='baseUrl'>
        /// Optional. Override the base URL for the Mailosaur server.
        /// </param>
        public MailosaurClient(string apiKey = null, string baseUrl = "https://mailosaur.com/")
        {
            var resolvedApiKey = string.IsNullOrWhiteSpace(apiKey)
                ? Environment.GetEnvironmentVariable("MAILOSAUR_API_KEY")
                : apiKey;

            if (string.IsNullOrWhiteSpace(resolvedApiKey))
            {
                throw new MailosaurException(
                    "'apiKey' must be set via the MAILOSAUR_API_KEY environment variable, or passed to the MailosaurClient constructor.",
                    "authentication_error");
            }

            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseUrl);
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("User-Agent", "mailosaur-dotnet/9.0.0");

            var apiKeyBytes = ASCIIEncoding.ASCII.GetBytes($"{resolvedApiKey}:");
            var apiKeyBase64 = Convert.ToBase64String(apiKeyBytes);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", apiKeyBase64);

            Servers = new Servers(_client);
            Messages = new Messages(_client);
            Files = new Files(_client);
            Analysis = new Analysis(_client);
            Usage = new Usage(_client);
            Devices = new Devices(_client);
            Previews = new Previews(_client);
        }

        /// <summary>
        /// Disposes relevant resources
        /// </summary>
        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
