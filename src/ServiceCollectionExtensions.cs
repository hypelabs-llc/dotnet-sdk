using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HypeLabs.Connect.Sdk;

/// <summary>
/// Registers <see cref="ConnectClient"/> in a dependency-injection container.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers a <see cref="ConnectClient"/> configured with the given <paramref name="configure"/> callback.
    /// The client is backed by <c>IHttpClientFactory</c>, so its handler is pooled and its lifetime managed for you.
    /// Inject <see cref="ConnectClient"/> anywhere afterwards.
    /// </summary>
    /// <example>
    /// <code>
    /// builder.Services.AddConnectClient(options => options.ApiKey = config["Connect:ApiKey"]!);
    /// </code>
    /// </example>
    public static IServiceCollection AddConnectClient(
        this IServiceCollection services,
        Action<ConnectClientOptions> configure)
    {
        services.AddOptions<ConnectClientOptions>()
            .Configure(configure)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services.AddConnectClientCore();
    }

    /// <summary>
    /// Registers a <see cref="ConnectClient"/> whose options are bound from configuration — pass the
    /// <c>Connect</c> section (or any section) so <c>ApiKey</c> and <c>BaseUrl</c> come from your settings.
    /// </summary>
    /// <example>
    /// <code>
    /// builder.Services.AddConnectClient(builder.Configuration.GetSection("Connect"));
    /// </code>
    /// </example>
    public static IServiceCollection AddConnectClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<ConnectClientOptions>()
            .Bind(configuration)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services.AddConnectClientCore();
    }

    private static IServiceCollection AddConnectClientCore(this IServiceCollection services)
    {
        // A named HttpClient the factory owns; ConnectClient sends its requests through it.
        services.AddHttpClient(nameof(ConnectClient));

        services.AddSingleton(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<ConnectClientOptions>>().Value;
            var httpClient = serviceProvider
                .GetRequiredService<IHttpClientFactory>()
                .CreateClient(nameof(ConnectClient));

            return new ConnectClient(options.ApiKey, httpClient, options.BaseUrl);
        });

        return services;
    }
}
