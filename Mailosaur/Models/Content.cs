namespace Mailosaur.Models
{
    public class Content
    {
        public bool Embed { get; set; }
        public bool Iframe { get; set; }
        public bool Object { get; set; }
        public bool Script { get; set; }
        public bool ShortUrls { get; set; }
        public int TextSize { get; set; }
        public int TotalSize { get; set; }
        public bool MissingAlt { get; set; }
        public bool MissingListUnsubscribe { get; set; }
    }
}