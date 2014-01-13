using System;
using System.Text;
using System.Linq;
using System.IO;
using NUnit.Framework;
using Newtonsoft.Json;
using System.Reflection;

namespace Mailosaur.Test
{
  [TestFixture()]
  public class Test
  {
    private static MailboxApi Mailbox;

    [Test()]
    public void ExpectedPublicMethods()
    {
      var type = typeof(Mailosaur.MailboxApi);
      var methods = new StringBuilder();
      foreach (var method in type.GetMembers().Where(m=>m.DeclaringType==type && m.MemberType== MemberTypes.Method).Cast<MethodInfo>())
      {
        methods.AppendFormat(@"{0} {1}({2})", method.ReturnType.Name, method.Name, String.Join(", ", method.GetParameters().Select(p => p.ParameterType.Name + " " + p.Name).ToArray()));
        methods.AppendLine();
      }
      Assert.AreEqual(File.ReadAllText("ExpectedMethods.txt"), methods.ToString(), "Client does not have expected methods.");
    }
    [TestFixtureSetUp]
    public void Setup()
    {
      var config = File.ReadAllLines(@"mailbox.settings");
      Mailbox = new MailboxApi(config [0], config [1]);
    }

    [Test]
    public void GetEmailsTest()
    {
      var emails = Mailbox.GetEmails();
      var expectedJson = File.ReadAllText("Emails.json").ToLowerInvariant();

      var settings = new JsonSerializerSettings{
        Formatting = Formatting.Indented,
        NullValueHandling = NullValueHandling.Ignore
      };
      var actual = JsonConvert.SerializeObject(emails, settings).ToLowerInvariant();

      // check if the reponse from the server serialized and deserialized properly
      Assert.AreEqual(expectedJson, actual);
    }

    [Test]
    public void GetEmailsSearchTest()
    {
      var emails = Mailbox.GetEmails("another");
      var expectedJson = File.ReadAllText("EmailsSearch.json").ToLowerInvariant();
      
      var settings = new JsonSerializerSettings{
        Formatting = Formatting.Indented,
        NullValueHandling = NullValueHandling.Ignore
      };
      var actual = JsonConvert.SerializeObject(emails, settings).ToLowerInvariant();
      
      // check if the reponse from the server serialized and deserialized properly
      Assert.AreEqual(expectedJson, actual);
    }

    [Test]
    public void GetEmailsByRecipientTest()
    {
      var emails = Mailbox.GetEmailsByRecipient("anything.1eaaeef6@clickity.me");
      var expectedJson = File.ReadAllText("EmailByRecipient.json").ToLowerInvariant();
      
      var settings = new JsonSerializerSettings{
        Formatting = Formatting.Indented,
        NullValueHandling = NullValueHandling.Ignore
      };
      var actual = JsonConvert.SerializeObject(emails, settings).ToLowerInvariant();
      
      // check if the reponse from the server serialized and deserialized properly
      Assert.AreEqual(expectedJson, actual);
    }

    [Test]
    public void GetRawEmail()
    {
      var emails = Mailbox.GetEmailsByRecipient("anything.1eaaeef6@clickity.me");
      var rawId = emails [0].RawId;

      var raw = Mailbox.GetRawEmail(rawId);
      Console.WriteLine(raw.Length);
    }

    [Test]
    public void GetAttachment()
    {
      var emails = Mailbox.GetEmailsByRecipient("anything.1eaaeef6@clickity.me");
      var id = emails [0].Attachments [0].Id;
      
      var attachment = Mailbox.GetAttachment(id);
      Console.WriteLine(attachment.Length);
    }
  }
}

