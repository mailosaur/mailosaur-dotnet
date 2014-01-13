using System;

namespace Mailosaur
{
  public class EmailData
  {
    public Link[] Links { get; set; }
    public Image[] Images { get; set; }
    public string Body { get; set; }

    public override string ToString()
    {
      return Body;
    }
  }
}

