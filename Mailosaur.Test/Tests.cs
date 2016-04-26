using System;
using System.Text;
using System.Linq;
using System.IO;
using System.Collections;
using System.Net.Mail;
using System.Net.Mime;
using NUnit.Framework;
using Newtonsoft.Json;
using System.Reflection;

namespace Mailosaur.Test
{
	[TestFixture]
	public class Test
	{
		private static MailboxApi Mailbox;
		private static string RecipientAddressShort;
		private static string RecipientAddressLong;

		[Test]
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

		[Test]
		public void GetEmailsTest()
		{
			var emails = Mailbox.GetEmails();
			Assert.AreNotEqual(0, emails.Length);
			AssertEmail(emails[0]);
		}

		[Test]
		public void GetEmailsSearchTest()
		{
			var emails = Mailbox.GetEmails("test");
			Assert.AreNotEqual(0, emails.Length);
			AssertEmail(emails[0]);
		}

		[Test]
		public void GetEmailsByRecipientTest()
		{
			var emails = Mailbox.GetEmailsByRecipient(RecipientAddressShort);
			Assert.AreNotEqual(0, emails.Length);
			AssertEmail(emails[0]);			
		}

		public void AssertEmail(Email email)
		{
			// html links:
			Assert.AreEqual(3, email.Html.Links.Length);
			Assert.AreEqual("https://mailosaur.com/", email.Html.Links[0].Href);
			Assert.AreEqual("mailosaur", email.Html.Links[0].Text);
			Assert.AreEqual("https://mailosaur.com/", email.Html.Links[1].Href);
			Assert.AreEqual(null, email.Html.Links[1].Text);
			Assert.AreEqual("http://invalid/", email.Html.Links[2].Href);
			Assert.AreEqual("invalid", email.Html.Links[2].Text);
      
			// html images:
			Assert.IsTrue(email.Html.Images[0].Src.EndsWith(".png") || email.Html.Images[0].Src.StartsWith("cid:"), email.Html.Images[0].Src);
			Assert.AreEqual("Inline image 1", email.Html.Images[0].Alt);
      
			// html body:
			var body = "<div dir=\"ltr\"><div style=\"font-family:arial,sans-serif;font-size:13px;color:rgb(80,0,80)\">this is a test.</div><div style=\"font-family:arial,sans-serif;font-size:13px;color:rgb(80,0,80)\"><br>this is a link: <a href=\"https://mailosaur.com/\" target=\"_blank\">mailosaur</a><br>\n</div><div class=\"gmail_quote\" style=\"font-family:arial,sans-serif;font-size:13px;color:rgb(80,0,80)\"><div dir=\"ltr\"><div><br></div><div>this is an image:<a href=\"https://mailosaur.com/\" target=\"_blank\"><img src=\"cid:ii_1435fadb31d523f6\" alt=\"Inline image 1\"></a></div>\n<div><br></div><div>this is an invalid link: <a href=\"http://invalid/\" target=\"_blank\">invalid</a></div></div></div>\n</div>";
			Assert.AreEqual(body, email.Html.Body);
      
			// text links:
			Assert.AreEqual(2, email.Text.Links.Length);
			Assert.AreEqual("https://mailosaur.com/", email.Text.Links[0].Href);
			Assert.AreEqual("https://mailosaur.com/", email.Text.Links[0].Text);
			Assert.AreEqual("https://mailosaur.com/", email.Text.Links[1].Href);
			Assert.AreEqual("https://mailosaur.com/", email.Text.Links[1].Text);
      
			// text body:
			var text = "this is a test.\n\nthis is a link: mailosaur <https://mailosaur.com/>\n\nthis is an image:[image: Inline image 1] <https://mailosaur.com/>\n\nthis is an invalid link: invalid";
      
			email.Text.Body = email.Text.Body.Replace("\r\n", "\n");
			Assert.AreEqual(text, email.Text.Body);
      
			// headers:
			Assert.AreEqual("\"anyone\" <anyone@example.com>", email.Headers.ContainsKey("From") ? email.Headers["From"].ToString() : email.Headers["from"].ToString());
			Assert.AreEqual("\"anybody\" <" + RecipientAddressShort + ">", email.Headers.ContainsKey("To") ? email.Headers["To"].ToString() : email.Headers["to"].ToString());
			Assert.IsNotNullOrEmpty(email.Headers.ContainsKey("Date") ? email.Headers["Date"].ToString() : email.Headers["date"].ToString());
			Assert.AreEqual("test subject", email.Headers.ContainsKey("Subject") ? email.Headers["Subject"].ToString() : email.Headers["subject"].ToString());
      
			// properties:
			Assert.AreEqual("test subject", email.Subject);
			Assert.AreEqual("normal", email.Priority);
      
			Assert.IsTrue(email.CreationDate.Year > 2013);
			Assert.IsNotNullOrEmpty(email.SenderHost);
			Assert.IsNotNullOrEmpty(email.Mailbox);
      
			// raw eml:
			Assert.IsNotNullOrEmpty(email.RawId);
			var eml = Mailbox.GetRawEmail(email.RawId);
			Assert.IsNotNull(eml);
			Assert.IsTrue(eml.Length > 1);
			var emlText = System.Text.Encoding.UTF8.GetString(eml);
			Assert.IsTrue(emlText.StartsWith("Received") || emlText.StartsWith("Authentication") || emlText.StartsWith("Date"));
      
      
			// from:
			Assert.AreEqual("anyone@example.com", email.From[0].Address);
			Assert.AreEqual("anyone", email.From[0].Name);
      
			// to:
			Assert.AreEqual(RecipientAddressShort, email.To[0].Address);
			Assert.AreEqual("anybody", email.To[0].Name);
      
			// attachments:
			Assert.AreEqual(2, email.Attachments.Length, "there should be 2 attachments");
      
			// attachment 1:
			var attachment1 = email.Attachments[0];
			Assert.IsNotNullOrEmpty(attachment1.Id);
			Assert.AreEqual(4819, attachment1.Length);

			Assert.AreEqual("image/png", attachment1.ContentType);
      
			//var data1 = Mailbox.GetAttachment(attachment1.Id);
			//Assert.IsNotNull(data1);
      
			// attachment 2:
			var attachment2 = email.Attachments[1];
			Assert.AreEqual(5260, attachment2.Length);
			Assert.AreEqual("logo-m-circle-sm.png", attachment2.FileName);
			Assert.AreEqual("image/png", attachment2.ContentType);
      
			var data2 = Mailbox.GetAttachment(attachment2.Id);
			Assert.IsNotNull(data2);
			Assert.AreEqual(attachment2.Length, data2.Length);

			
		}

		[TestFixtureSetUp]
		public void Setup()
		{
			var config = File.ReadAllLines(@"mailbox.settings");
			var mailboxid = Environment.GetEnvironmentVariable("MAILBOXID") ?? config[0];
			var apikey = Environment.GetEnvironmentVariable("APIKEY") ?? config[1];

			var host = Environment.GetEnvironmentVariable("HOST") ?? "mailosaur.in";
			var port = Environment.GetEnvironmentVariable("PORT") ?? "25";
			

			MailboxApi.BaseUrl = Environment.GetEnvironmentVariable("BASEURI") ?? MailboxApi.BaseUrl;			
			Mailbox = new MailboxApi(mailboxid, apikey);

			Mailbox.DeleteAllEmail();
		
			// send test email:      
			string from = "anyone<anyone@example.com>",
			subject = "test subject",
			html = "<div dir=\"ltr\"><div style=\"font-family:arial,sans-serif;font-size:13px;color:rgb(80,0,80)\">this is a test.</div><div style=\"font-family:arial,sans-serif;font-size:13px;color:rgb(80,0,80)\"><br>this is a link: <a href=\"https://mailosaur.com/\" target=\"_blank\">mailosaur</a><br>\n</div><div class=\"gmail_quote\" style=\"font-family:arial,sans-serif;font-size:13px;color:rgb(80,0,80)\"><div dir=\"ltr\"><div><br></div><div>this is an image:<a href=\"https://mailosaur.com/\" target=\"_blank\"><img src=\"cid:ii_1435fadb31d523f6\" alt=\"Inline image 1\"></a></div>\n<div><br></div><div>this is an invalid link: <a href=\"http://invalid/\" target=\"_blank\">invalid</a></div></div></div>\n</div>",
			text = "this is a test.\r\n\r\nthis is a link: mailosaur <https://mailosaur.com/>\r\n\r\nthis is an image:[image: Inline image 1] <https://mailosaur.com/>\n\nthis is an invalid link: invalid";

			RecipientAddressShort = Mailbox.GenerateEmailAddress();
			RecipientAddressLong = "anybody<" + RecipientAddressShort + ">";

			var message = new MailMessage();
			message.Subject = subject;
			message.To.Add(RecipientAddressLong);
			message.From = new MailAddress(from);

			// set text body:
			message.Body = text;
			message.IsBodyHtml = false;
			message.BodyEncoding = Encoding.UTF8;

			// add html 
			var htmlView = AlternateView.CreateAlternateViewFromString(html, new ContentType(MediaTypeNames.Text.Html));
			htmlView.TransferEncoding = TransferEncoding.Base64;

			message.AlternateViews.Add(htmlView);

			// inline image:
			var image = new LinkedResource("logo-m.png");
			image.ContentId = "ii_1435fadb31d523f6";
			htmlView.LinkedResources.Add(image);

			// add attachment image:
			message.Attachments.Add(new System.Net.Mail.Attachment("logo-m-circle-sm.png"));                

			var client = new SmtpClient();
			client.Host = host;
			client.Port = int.Parse(port);

			try
			{
				client.Send(message);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			// wait to ensure email has arrived.
			System.Threading.Thread.Sleep(3000);
		}
	}
}

