using System;

namespace Mailosaur
{
  public class Image
  {
    public string Src { get; set; }
    public string Alt { get; set; }

    public override string ToString()
    {
      return Src;
    }
   
  }
}

