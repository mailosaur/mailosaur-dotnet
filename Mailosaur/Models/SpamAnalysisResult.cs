namespace Mailosaur.Models
{
    public partial class SpamAnalysisResult
    {
        public SpamFilterResults SpamFilterResults { get; set; }
        public double? Score { get; set; }

    }
}
