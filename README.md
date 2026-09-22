<p align="center">
  <img src="https://i.imgur.com/NtKvmk2.png" height="100" alt="HypeLabs" />
</p>
<h3 align="center">
  HypeLabs Connect SDK
</h3>
<p align="center">
  The C# client for the HypeLabs Connect API, for internal .NET services. 🚀
</p>
<p align="center">
  <a href="https://www.nuget.org/packages/HypeLabs.Connect.Sdk"><img src="https://img.shields.io/nuget/v/HypeLabs.Connect.Sdk?color=512bd4&label=NuGet" /></a>
  <a href="https://www.nuget.org/packages/HypeLabs.Connect.Sdk"><img src="https://img.shields.io/nuget/dt/HypeLabs.Connect.Sdk?color=512bd4&label=Downloads" /></a>
  <a href="https://connect.hypelabs.network"><img src="https://img.shields.io/badge/API-connect.hypelabs.network-6366f1" /></a>
  <a href="https://learn.microsoft.com/openapi/kiota/"><img src="https://img.shields.io/badge/Generated%20With-Kiota-0078d4" /></a>
  <a href="LICENSE"><img src="https://img.shields.io/badge/License-MIT-lightgrey.svg" /></a>
</p>

## Requirements

- .NET 10.0 or later
- A HypeLabs Connect API key (`hl_live_…`)

## Install

1. **Add the package:**

   ```bash
   dotnet add package HypeLabs.Connect.Sdk
   ```

2. **Create your organization** on the [HypeLabs Console](https://console.hypelabs.network/) and generate an API key.

## Usage

### With dependency injection (recommended)

```csharp
builder.Services.AddConnectClient(options => options.ApiKey = builder.Configuration["Connect:ApiKey"]!);

// …then inject it anywhere:
public class MyService(ConnectClient client)
{
    public Task<List<Customer>?> GetCustomers() => client.Customers.GetAsync();
}
```

Options can also be bound from configuration:

```csharp
builder.Services.AddConnectClient(builder.Configuration.GetSection("Connect"));
```

`appsettings.json` — `BaseUrl` is optional and defaults to `https://connect.hypelabs.network`:

```json
{
  "Connect": {
    "ApiKey": "hl_live_…"
  }
}
```

### Standalone

```csharp
var client = new ConnectClient("hl_live_…");
var customers = await client.Customers.GetAsync();
```

The API is authenticated with the `X-Api-Key` header, wired up internally — you never touch the Kiota plumbing.
