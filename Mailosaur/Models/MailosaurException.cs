using System;

namespace Mailosaur.Models
{
    /// <summary>
    /// Exception thrown for an invalid response with MailosaurError
    /// information.
    /// </summary>
    public class MailosaurException : Exception
    {
        public string ErrorType { get; private set; }
        public int? HttpStatusCode { get; private set; }
        public string HttpResponseBody { get; private set; }

        public MailosaurException()
        {
        }

        public MailosaurException(string message, string errorType)
            : base(message)
        {
            ErrorType = errorType;
        }

        public MailosaurException(string message, string errorType, int httpStatusCode, string httpResponseBody)
            : base(message)
        {
            ErrorType = errorType;
            HttpStatusCode = httpStatusCode;
            HttpResponseBody = httpResponseBody;
        }

        public MailosaurException(string message, Exception inner)
            : base(message, inner)
        {
            ErrorType = "client_error";
        }
    }
}
