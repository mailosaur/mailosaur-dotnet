namespace Mailosaur.Models
{
    using System.Collections.Generic;

    public class MessageContent
    {
        public IList<Link> Links { get; set; }
        public IList<Code> Codes { get; set; }
        public IList<Image> Images { get; set; }
        public string Body { get; set; }

    }
}
