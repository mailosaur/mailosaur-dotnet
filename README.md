# [Mailosaur - .NET library](https://mailosaur.com/) &middot; [![](https://github.com/mailosaur/mailosaur-dotnet/workflows/CI/badge.svg)](https://github.com/mailosaur/mailosaur-dotnet/actions) 

Mailosaur lets you automate email and SMS tests as part of software development and QA.

- **Unlimited test email addresses for all**  - every account gives users an unlimited number of test email addresses to test with.
- **End-to-end (e2e) email and SMS testing** Allowing you to set up end-to-end tests for password reset emails, account verification processes and MFA/one-time passcodes sent via text message.
- **Fake SMTP servers** Mailosaur also provides dummy SMTP servers to test with; allowing you to catch email in staging environments - preventing email being sent to customers by mistake.

## Get Started

This guide provides several key sections:

  - [Get Started](#get-started)
  - [Creating an account](#creating-an-account)
  - [Test email addresses with Mailosaur](#test-email-addresses-with-mailosaur)
  - [Find an email](#find-an-email)
  - [Find an SMS message](#find-an-sms-message)
  - [Testing plain text content](#testing-plain-text-content)
  - [Testing HTML content](#testing-html-content)
  - [Working with hyperlinks](#working-with-hyperlinks)
  - [Working with attachments](#working-with-attachments)
  - [Working with images and web beacons](#working-with-images-and-web-beacons)
  - [Spam checking](#spam-checking)

You can find the full [Mailosaur documentation](https://mailosaur.com/docs/) on the website.

If you get stuck, just contact us at support@mailosaur.com.

### Installation

Install the Mailosaur NuGet package via the command line:

```sh
dotnet add package mailosaur
```

From Package Manager:

```
PM> Install-Package mailosaur
```

From within Visual Studio:

1. Open the Solution Explorer.
2. Right-click on a project within your solution.
3. Click on Manage NuGet Packages...
4. Click on the Browse tab and search for "Mailosaur".
5. Click on the Mailosaur package, select the appropriate version in the right-tab and click *Install*.

### API Reference

This library is powered by the Mailosaur [email & SMS testing API](https://mailosaur.com/docs/api/). You can easily check out the API itself by looking at our [API reference documentation](https://mailosaur.com/docs/api/) or via our Postman or Insomnia collections:

[![Run in Postman](https://run.pstmn.io/button.svg)](https://app.getpostman.com/run-collection/6961255-6cc72dff-f576-451a-9023-b82dec84f95d?action=collection%2Ffork&collection-url=entityId%3D6961255-6cc72dff-f576-451a-9023-b82dec84f95d%26entityType%3Dcollection%26workspaceId%3D386a4af1-4293-4197-8f40-0eb49f831325)
 [![Run in Insomnia}](https://insomnia.rest/images/run.svg)](https://insomnia.rest/run/?label=Mailosaur&uri=https%3A%2F%2Fmailosaur.com%2Finsomnia.json)

## Creating an account

Create a [free trial account](https://mailosaur.com/app/signup) for Mailosaur via the website.

Once you have this, navigate to the [API tab](https://mailosaur.com/app/project/api) to find the following values:

- **Server ID** - Servers act like projects, which group your tests together. You need this ID whenever you interact with a server via the API.
- **Server Domain** - Every server has its own domain name. You'll need this to send email to your server.
- **API Key** - You can create an API key per server (recommended), or an account-level API key to use across your whole account. [Learn more about API keys](https://mailosaur.com/docs/managing-your-account/api-keys/).

## Test email addresses with Mailosaur

Mailosaur gives you an **unlimited number of test email addresses** - with no setup or coding required!

Here's how it works:

* When you create an account, you are given a server.
* Every server has its own **Server Domain** name (e.g. `abc123.mailosaur.net`)
* Any email address that ends with `@{YOUR_SERVER_DOMAIN}` will work with Mailosaur without any special setup. For example:
  * `build-423@abc123.mailosaur.net`
  * `john.smith@abc123.mailosaur.net`
  * `rAnDoM63423@abc123.mailosaur.net`
* You can create more servers when you need them. Each one will have its own domain name.

***Can't use test email addresses?** You can also [use SMTP to test email](https://mailosaur.com/docs/email-testing/sending-to-mailosaur/#sending-via-smtp). By connecting your product or website to Mailosaur via SMTP, Mailosaur will catch all email your application sends, regardless of the email address.*

## Find an email

In automated tests you will want to wait for a new email to arrive. This library makes that easy with the `Messages.Get` method. Here's how you use it:

```csharp
using System;
using Mailosaur;
using Mailosaur.Models;

namespace EmailTests
{
  class Program
  {
    static void Main(string[] args)
    {
      var mailosaur = new MailosaurClient("API_KEY");

      // See https://mailosaur.com/app/project/api
      var serverId = "abc123";
      var serverDomain = "abc123.mailosaur.net";

      var criteria = new SearchCriteria() {
          SentTo = "anything@" + serverDomain
      };

      var message = mailosaur.Messages.Get(serverId, criteria);

      Console.WriteLine(email.Subject); // "Hello world!"
    }
  }
}
```

### What is this code doing?

1. Sets up an instance of `MailosaurClient` with your API key.
2. Waits for an email to arrive at the server with ID `abc123`.
3. Outputs the subject line of the email.

### My email wasn't found

First, check that the email you sent is visible in the [Mailosaur Dashboard](https://mailosaur.com/api/project/messages). 

If it is, the likely reason is that by default, `Messages.Get` only searches emails received by Mailosaur in the last 1 hour. You can override this behavior (see the `receivedAfter` option below), however we only recommend doing this during setup, as your tests will generally run faster with the default settings:

```csharp
// Override receivedAfter to search all messages since Jan 1st
var email = mailosaur.Messages.Get("SERVER_ID", new SearchCriteria() {
  SentTo = "someone@SERVER_ID.mailosaur.net"
}, receivedAfter: new DateTime(2021, 1, 1));
```

## Find an SMS message

**Important:** Trial accounts do not automatically have SMS access. Please contact our support team to enable a trial of SMS functionality.

If your account has [SMS testing](https://mailosaur.com/sms-testing/) enabled, you can reserve phone numbers to test with, then use the Mailosaur API in a very similar way to when testing email:

```csharp
var mailosaur = new MailosaurClient("API_KEY");

var serverId = "abc123";

var criteria = new SearchCriteria() {
    SentTo = "4471235554444"
};

var sms = mailosaur.Messages.Get(serverId, criteria);

Console.WriteLine(sms.Text.Body);
```

## Testing plain text content

Most emails, and all SMS messages, should have a plain text body. Mailosaur exposes this content via the `Text.Body` property on an email or SMS message:

```csharp
Console.WriteLine(message.Text.Body); // "Hi Jason, ..."

if (message.Text.Body.Contains("Jason"))
{
  Console.WriteLine('Email contains "Jason"');
}
```

### Extracting verification codes from plain text

You may have an email or SMS message that contains an account verification code, or some other one-time passcode. You can extract content like this using a simple regex.

Here is how to extract a 6-digit numeric code:

```csharp
Console.WriteLine(message.Text.Body); // "Your access code is 243546."

var matches = Regex.Matches(message.Text.Body, "([0-9]{6})", RegexOptions.IgnoreCase);

Console.WriteLine(matches[0]); // "243546"
```

[Read more](https://mailosaur.com/docs/test-cases/text-content/)

## Testing HTML content

Most emails also have an HTML body, as well as the plain text content. You can access HTML content in a very similar way to plain text:

```csharp
Console.WriteLine(message.Html.Body); // "<html><head ..."
```

### Working with HTML using HtmlAgilityPack

If you need to traverse the HTML content of an email. For example, finding an element via a CSS selector, you can use the [HtmlAgilityPack](https://html-agility-pack.net/) library.

```sh
dotnet add package HtmlAgilityPack
```

```csharp
var doc = new HtmlDocument();
doc.LoadHtml(message.Html.Body);

// Find element using xpath
var node = doc.DocumentNode.SelectSingleNode("//*[contains(@class, 'verification-code')]")

var verificationCode = node.InnerText;

Console.WriteLine(verificationCode); // "542163"
```

[Read more](https://mailosaur.com/docs/test-cases/html-content/)

## Working with hyperlinks

When an email is sent with an HTML body, Mailosaur automatically extracts any hyperlinks found within anchor (`<a>`) and area (`<area>`) elements and makes these viable via the `Html.Links` array.

Each link has a text property, representing the display text of the hyperlink within the body, and an href property containing the target URL:

```csharp
// How many links?
Console.WriteLine(message.Html.Links.Count); // 2

var firstLink = message.Html.Links[0];
Console.WriteLine(firstLink.Text); // "Google Search"
Console.WriteLine(firstLink.Href); // "https://www.google.com/"
```

**Important:** To ensure you always have valid emails. Mailosaur only extracts links that have been correctly marked up with `<a>` or `<area>` tags.

### Links in plain text (including SMS messages)

Mailosaur auto-detects links in plain text content too, which is especially useful for SMS testing:

```csharp
// How many links?
Console.WriteLine(message.Text.Links.Count); // 2

var firstLink = message.Text.Links[0];
Console.WriteLine(firstLink.Href); // "https://www.google.com/"
```

## Working with attachments

If your email includes attachments, you can access these via the `Attachments` property:

```csharp
// How many attachments?
Console.WriteLine(message.Attachments.Count); // 2
```

Each attachment contains metadata on the file name and content type:

```csharp
var firstAttachment = message.Attachments[0];
Console.WriteLine(firstAttachment.FileName); // "contract.pdf"
Console.WriteLine(firstAttachment.ContentType); // "application/pdf"
```

The `Length` property returns the size of the attached file (in bytes):

```csharp
var firstAttachment = message.Attachments[0];
Console.WriteLine(firstAttachment.Length); // 4028
```

## Working with images and web beacons

The `Html.Images` property of a message contains an array of images found within the HTML content of an email. The length of this array corresponds to the number of images found within an email:

```csharp
// How many images in the email?
Console.WriteLine(message.Html.Images.Count); // 1
```

### Remotely-hosted images

Emails will often contain many images that are hosted elsewhere, such as on your website or product. It is recommended to check that these images are accessible by your recipients.

All images should have an alternative text description, which can be checked using the `Alt` attribute.

```csharp
var image = message.Html.Images[0];
Console.WriteLine(image.Alt); // "Hot air balloon"
```

### Triggering web beacons

A web beacon is a small image that can be used to track whether an email has been opened by a recipient.

Because a web beacon is simply another form of remotely-hosted image, you can use the `Src` attribute to perform an HTTP request to that address:

```csharp
var image = message.Html.Images[0];

Console.WriteLine(image.Src); // "https://example.com/s.png?abc123"

// Make an HTTP call to trigger the web beacon
using (var client = new HttpClient()) {
  var result = await client.GetAsync(image.Src);
  Console.WriteLine(result.StatusCode); // 200
}
```

## Spam checking

You can perform a [SpamAssassin](https://spamassassin.apache.org/) check against an email. The structure returned matches the [spam test object](https://mailosaur.com/docs/api/#spam):

```csharp
var result = mailosaur.Analysis.Spam(message.Id);
          
Console.WriteLine(result.Score); // 0.5

foreach (var r in result.SpamFilterResults.SpamAssassin) {
  Console.WriteLine(r.Rule);
  Console.WriteLine(r.Description);
  Console.WriteLine(r.Score);
}
```

## Development

The test suite requires the following environment variables to be set:

```sh
export MAILOSAUR_API_KEY=your_api_key
export MAILOSAUR_SERVER=server_id
```

Run all tests:

```sh
dotnet test
```

## Contacting us

You can get us at [support@mailosaur.com](mailto:support@mailosaur.com)
