using System;

namespace Mailosaur.Models
{
    public class OtpResult
    {
        /// <summary>
        /// The current one-time password.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The expiry date/time of the current one-time password.
        /// </summary>
        public System.DateTime Expires { get; set; }

    }
}
