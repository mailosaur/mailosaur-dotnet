using System;

namespace Mailosaur
{
  public class Attachment
  {
    public string ContentType { get; set; }
    public string FileName { get; set; }
    public long Length { get; set; }
    public string Id { get; set; }

    public override string ToString()
    {
      return Id;
    }
  }
}

