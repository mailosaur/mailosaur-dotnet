using System;

namespace Mailosaur.Models
{
    /// <summary>
    /// Exception thrown for an invalid response with MailosaurError
    /// information.
    /// </summary>
    public class MailosaurException : Exception
    {
        /// <summary>
        /// Gets or sets the body object.
        /// </summary>
        public MailosaurError MailosaurError { get; set; }

        public MailosaurException()
        {
        }

        public MailosaurException(string message)
            : base(message)
        {
            MailosaurError = new MailosaurError() {
                Type = "ValidationError"
            };
        }

        public MailosaurException(string message, Exception inner)
            : base(message, inner)
        {
            MailosaurError = new MailosaurError() {
                Type = "ValidationError"
            };
        }
    }
}
