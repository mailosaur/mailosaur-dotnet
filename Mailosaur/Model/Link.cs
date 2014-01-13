using System;

namespace Mailosaur
{
  public class Link
  {
    public string Href { get; set; }
    public string Text { get; set; }

    public override string ToString()
    {
      return Href;
    }
  }
}

