namespace Mailosaur.Models
{
    public class Attachment
    {
        public string Id { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
        public string ContentId { get; set; }
        public int? Length { get; set; }
        public string Url { get; set; }
    }
}
