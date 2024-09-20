using System;
using System.Collections.Generic;

namespace Mailosaur.Models
{
    public class EmailAuthenticationResult
    {
        public ResultEnum Result { get; set; }
        public String Description { get; set; }
        public string RawValue { get; set; }
        public Dictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();
    }
}