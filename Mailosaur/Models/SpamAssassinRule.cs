namespace Mailosaur.Models
{
    public partial class SpamAssassinRule
    {
        public double? Score { get; set; }
        public string Rule { get; set; }
        public string Description { get; set; }

    }
}
