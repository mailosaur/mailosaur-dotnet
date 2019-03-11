namespace Mailosaur.Models
{
    using System.Collections.Generic;

    public partial class SpamFilterResults
    {
        public IList<SpamAssassinRule> SpamAssassin { get; set; }

    }
}
