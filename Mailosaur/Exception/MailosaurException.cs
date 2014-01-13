using System;

namespace Mailosaur
{
  public class MailosaurException : Exception
  {
    public MailosaurException() : base()
    {
    }
    public MailosaurException(string message) : base(message)
    {
    }
    public MailosaurException(string message, Exception innerException) : base(message, innerException)
    {
    }
  }
}

