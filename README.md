# Mailosaur .NET Client Library

[Mailosaur](https://mailosaur.com) allows you to automate tests involving email. Allowing you to perform end-to-end automated and functional email testing.

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

## Documentation and usage examples

[Mailosaur's documentation](https://mailosaur.com/docs) includes all the information and usage examples you'll need.

## Running tests

Once you've cloned this repository locally, you can simply run:

```
cd Mailosaur.Test

dotnet restore

export MAILOSAUR_API_KEY=your_api_key
export MAILOSAUR_SERVER=server_id

dotnet test
```

## Contacting us

You can get us at [support@mailosaur.com](mailto:support@mailosaur.com)
