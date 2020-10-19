# Mailosaur .NET Client Library

[Mailosaur](https://mailosaur.com) lets you automate email and SMS tests, like account verification and password resets, and integrate these into your CI/CD pipeline.

[![](https://github.com/mailosaur/mailosaur-dotnet/workflows/CI/badge.svg)](https://github.com/mailosaur/mailosaur-dotnet/actions)

## Installation

### Install Mailosaur via NuGet

From the command line:

```
nuget install mailosaur
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

## Documentation

Please see the [.NET client reference](https://mailosaur.com/docs/email-testing/dotnet/client-reference/) for the most up-to-date documentation.

## Usage

Example.cs

```csharp
using System;
using Mailosaur;

namespace example
{
  class Program
  {
    static void Main(string[] args)
    {
      var mailosaur = new MailosaurClient("YOUR_API_KEY");

      var result = mailosaur.Servers.List();

      Console.WriteLine("Your have a server called: " + result.Items[0].Name);
    }
  }
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
