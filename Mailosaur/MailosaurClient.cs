namespace Mailosaur
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using Mailosaur.Models;
    using Mailosaur.Operations;

    /// <summary>
    /// The Mailosaur client — the main entry point to the Mailosaur API. Construct an instance with
    /// your API key (or set the <c>MAILOSAUR_API_KEY</c> environment variable), then use the operations
    /// namespaces (<see cref="Messages"/>, <see cref="Servers"/>, <see cref="Files"/>, <see cref="Devices"/>,
    /// <see cref="Analysis"/>, <see cref="Previews"/>, <see cref="Usage"/>) to automate email and SMS testing.
    /// </summary>
    public class MailosaurClient : IDisposable
    {
        /// <summary>
        /// Operations for creating and managing your Mailosaur servers (virtual inboxes).
        /// </summary>
        public Servers Servers { get; private set; }

        /// <summary>
        /// Operations for finding, retrieving, creating, and managing email and SMS messages.
        /// </summary>
        public Messages Messages { get; private set; }

        /// <summary>
        /// Operations for downloading attachments, EML source, and email preview screenshots.
        /// </summary>
        public Files Files { get; private set; }

        /// <summary>
        /// Operations for analyzing email content and deliverability, including spam scoring.
        /// </summary>
        public Analysis Analysis { get; private set; }

        /// <summary>
        /// Operations for inspecting account usage limits and recent transactional usage.
        /// </summary>
        public Usage Usage { get; private set; }

        /// <summary>
        /// Operations for managing virtual security devices and retrieving their one-time passwords.
        /// </summary>
        public Devices Devices { get; private set; }

        /// <summary>
        /// Operations for discovering the email clients available for generating email previews.
        /// </summary>
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
