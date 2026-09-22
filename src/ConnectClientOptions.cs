using System.ComponentModel.DataAnnotations;

namespace HypeLabs.Connect.Sdk;

/// <summary>
/// Configuration for the Connect API client, bound from configuration (e.g. a <c>Connect</c> section) or set
/// inline when calling <c>AddConnectClient</c>.
/// </summary>
public sealed class ConnectClientOptions
{
    /// <summary>The Connect API key used to authenticate every request (e.g. <c>hl_live_…</c>). Required.</summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "A Connect API key is required.")]
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// The API base URL. Leave null to use the SDK's default (<c>https://connect.hypelabs.network</c>); override
    /// only to point at a staging or local instance.
    /// </summary>
    public string? BaseUrl { get; set; }
}
