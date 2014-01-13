using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Mailosaur
{
  public class Email
  {
    public EmailData Html { get; set; }
    public EmailData Text { get; set; }
    public Dictionary<String,Object> Headers { get; set; }
    public string Subject { get; set; }
    public string Priority { get; set; }
    public EmailAddress[] From { get; set; }
    public EmailAddress[] To { get; set; }
    public Attachment[] Attachments { get; set; }
    public DateTime CreationDate { get; set; }
    public string SenderHost { get; set; }
    public string Mailbox { get; set; }
    public string Id { get; set; }
    public string RawId { get; set; }

    public override string ToString()
    {
      return Id;
    }
  }
}

