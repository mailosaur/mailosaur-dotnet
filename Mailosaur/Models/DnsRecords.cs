using System.Collections.Generic;

namespace Mailosaur.Models
{
    public class DnsRecords
    {
        public List<string> A { get; set; }
        public List<string> MX { get; set; }
        public List<string> PTR { get; set; }
    }
}