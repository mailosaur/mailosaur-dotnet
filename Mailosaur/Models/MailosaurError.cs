namespace Mailosaur.Models
{
    using System.Collections.Generic;

    public class MailosaurError
    {
        /// <summary>
        /// Gets or sets possible values include: 'None', 'ValidationError',
        /// 'AuthenticationError', 'PermissionDeniedError',
        /// 'ResourceNotFoundError'
        /// </summary>
        public string Type { get; set; }
        public IDictionary<string, string> Messages { get; set; }
        public object Model { get; set; }

    }
}
