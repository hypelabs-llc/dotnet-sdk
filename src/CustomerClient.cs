using HypeLabs.Connect.Sdk.GeneratedCustomer;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace HypeLabs.Connect.Sdk;

/// <summary>
/// The entry point to the HypeLabs Connect API as one of your end users. Construct one without a token to sign a
/// customer in or up (the authenticate and sign-up endpoints are anonymous), then construct one with the returned
/// token to reach the rest — <c>client.Orders.GetAsync()</c>, <c>client.Proxies.GetAsync()</c>, and so on.
///
/// <code>
/// var auth = new CustomerClient(organizationId: organizationId);
/// var session = await auth.Customers.Authenticate.PostAsync(new() { Email = email, Password = password });
/// var client = new CustomerClient(session.Token, organizationId);
/// var orders = await client.Orders.GetAsync();
/// </code>
///
/// The Kiota request adapter, the <c>Authorization: Bearer</c> authentication and the <c>X-Organization-Id</c>
/// header are wired up internally, so callers never touch the generated <see cref="CustomerApiClient"/> plumbing
/// directly.
/// </summary>
public sealed class CustomerClient : CustomerApiClient
{
    /// <summary>
    /// Creates a customer client, with an internally-managed <see cref="HttpClient"/>. Prefer
    /// <c>AddCustomerClient</c> in a DI-based app so the HTTP handler is pooled by the factory.
    /// </summary>
    /// <param name="token">A customer token from one of the authenticate endpoints; omit it before signing in.</param>
    /// <param name="organizationId">The organization the customer belongs to, sent as <c>X-Organization-Id</c>.</param>
    /// <param name="baseUrl">Optional API base URL; leave null to use the SDK default.</param>
    public CustomerClient(string? token = null, string? organizationId = null, string? baseUrl = null)
        : base(new HttpClientRequestAdapter(new CustomerAuthenticationProvider(token, organizationId)))
    {
        if (!string.IsNullOrWhiteSpace(baseUrl))
            RequestAdapter.BaseUrl = baseUrl;
    }

    /// <summary>
    /// Creates a customer client that sends its requests through the supplied <see cref="HttpClient"/> — the
    /// constructor used by the DI factory, which owns the client's lifetime and handler pooling.
    /// </summary>
    /// <param name="httpClient">The HTTP client to send requests with, typically from <c>IHttpClientFactory</c>.</param>
    /// <param name="token">A customer token from one of the authenticate endpoints; omit it before signing in.</param>
    /// <param name="organizationId">The organization the customer belongs to, sent as <c>X-Organization-Id</c>.</param>
    /// <param name="baseUrl">Optional API base URL; leave null to use the SDK default.</param>
    public CustomerClient(HttpClient httpClient, string? token = null, string? organizationId = null, string? baseUrl = null)
        : base(new HttpClientRequestAdapter(
            new CustomerAuthenticationProvider(token, organizationId),
            httpClient: httpClient))
    {
        if (!string.IsNullOrWhiteSpace(baseUrl))
            RequestAdapter.BaseUrl = baseUrl;
    }
}
