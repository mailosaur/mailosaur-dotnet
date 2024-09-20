using System.Collections.Generic;

namespace Mailosaur.Models
{
    public class DeliverabilityReport
    {
        public EmailAuthenticationResult Spf { get; set; }
        public List<EmailAuthenticationResult> Dkim { get; set; }
        public EmailAuthenticationResult Dmarc { get; set; }
        public List<BlockListResult> BlockLists { get; set; }
        public Content Content { get; set; }
        public DnsRecords DnsRecords { get; set; } = new DnsRecords();
        public SpamAssassinResult SpamAssassin { get; set; }
    }
}