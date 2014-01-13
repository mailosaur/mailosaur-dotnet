using System;

namespace Mailosaur
{
  public class EmailAddress
  {
    public string Address { get; set; }
    public string Name { get; set; }

    public override string ToString()
    {
      return string.IsNullOrEmpty(Name) ? 
				Address :
				string.Format("{0} <{1}>", Name, Address);
    }
  }
}

