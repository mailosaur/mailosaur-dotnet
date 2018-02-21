# Mailosaur .NET Client Library

[Mailosaur](https://mailosaur.com) allows you to automate tests involving email. Allowing you to perform end-to-end automated and functional email testing.

[![Build Status](https://travis-ci.org/mailosaur/mailosaur-dotnet.svg?branch=master)](https://travis-ci.org/mailosaur/mailosaur-dotnet)

## Installation

### Install Mailosaur via NuGet

From the command line:

```
nuget install mailosaur
```

From Package Manager:

```
PM> Install-Package Stripe.net
```

From within Visual Studio:

1. Open the Solution Explorer.
2. Right-click on a project within your solution.
3. Click on Manage NuGet Packages...
4. Click on the Browse tab and search for "Mailosaur".
5. Click on the Mailosaur package, select the appropriate version in the right-tab and click *Install*.

## Documentation and usage examples

[Mailosaur's documentation](https://mailosaur.com/docs) includes all the information and usage examples you'll need.

## Building

1. Install [Node.js](https://nodejs.org/) (LTS)

2. Install [AutoRest](https://github.com/Azure/autorest) using `npm`

```
# Depending on your configuration you may need to be elevated or root to run this. (on OSX/Linux use 'sudo')
npm install -g autorest
```

3. Run the build script

```
./build.sh
```

### AutoRest Configuration

This project uses [AutoRest](https://github.com/Azure/autorest), below is the configuration that the `autorest` command will automatically pick up.

> see https://aka.ms/autorest

```yaml
input-file: https://next.mailosaur.com/swagger/latest/swagger.json
```

```yaml
csharp:
    output-folder: Mailosaur
    add-credentials: true
    sync-methods: essential
    use-internal-constructors: true
    override-client-name: MailosaurClient
    namespace: Mailosaur
```

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
