namespace Mailosaur
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using Mailosaur.Operations;

    public class MailosaurClient
    {
        public Servers Servers { get; private set; }
        public Messages Messages { get; private set; }
        public Files Files { get; private set; }
        public Analysis Analysis { get; private set; }

        private readonly HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the MailosaurClient class.
        /// </summary>
        /// <param name='apiKey'>
        /// Your Mailosaur API key.
        /// </param>
        public MailosaurClient(string apiKey) : this(apiKey, null) {}

        /// <summary>
        /// Initializes a new instance of the MailosaurClient class.
        /// </summary>
        /// <param name='apiKey'>
        /// Your Mailosaur API key.
        /// </param>
        /// <param name='baseUrl'>
        /// Optional. Override the base URL for the mailosaur server.
        /// </param>
        public MailosaurClient(string apiKey, string baseUrl = "https://mailosaur.com/")
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseUrl);
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("User-Agent", "mailosaur-dotnet/5.0.2");
            
            var apiKeyBytes = ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:", apiKey));
            var apiKeyBase64 = Convert.ToBase64String(apiKeyBytes);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", apiKeyBase64);

            Servers = new Servers(_client);
            Messages = new Messages(_client);
            Files = new Files(_client);
            Analysis = new Analysis(_client);
        }
    }
}
