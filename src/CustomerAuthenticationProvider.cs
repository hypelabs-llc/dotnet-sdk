using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

namespace HypeLabs.Connect.Sdk;

/// <summary>
/// Authenticates a customer request: a bearer token once the customer is signed in, and the organization id the
/// customer belongs to. Both are optional — before sign-in there's no token yet, and the organization is only
/// needed where the request doesn't already carry it (the anonymous sign-up names its organization by header).
/// </summary>
internal sealed class CustomerAuthenticationProvider(string? token, string? organizationId) : IAuthenticationProvider
{
    public Task AuthenticateRequestAsync(
        RequestInformation request,
        Dictionary<string, object>? additionalAuthenticationContext = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.TryAdd("Authorization", $"Bearer {token}");

        if (!string.IsNullOrWhiteSpace(organizationId))
            request.Headers.TryAdd("X-Organization-Id", organizationId);

        return Task.CompletedTask;
    }
}
