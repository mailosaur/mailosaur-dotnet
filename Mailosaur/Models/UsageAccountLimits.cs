namespace Mailosaur.Models
{
    public class UsageAccountLimits
    {
        public UsageAccountLimit Servers { get; set; }
        public UsageAccountLimit Users { get; set; }
        public UsageAccountLimit Email { get; set; }
        public UsageAccountLimit Sms { get; set; }
    }
}
