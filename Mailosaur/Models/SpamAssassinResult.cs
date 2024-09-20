using System.Collections.Generic;

namespace Mailosaur.Models
{
    public class SpamAssassinResult
    {
        public int Score { get; set; }
        public ResultEnum Result { get; set; }
        public List<SpamAssassinRule> Rules { get; set; }
    }
}