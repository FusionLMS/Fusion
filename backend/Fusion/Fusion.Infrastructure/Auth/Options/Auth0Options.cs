using System.Diagnostics.CodeAnalysis;

namespace Fusion.Infrastructure.Auth.Options;

/// <summary>
/// Represents the configuration options required for Auth0 authentication.
/// </summary>
[ExcludeFromCodeCoverage]
public record Auth0Options
{
    /// <summary>
    /// Gets the configuration section name in the application settings.
    /// Use this constant to access the Swagger Contacts configuration.
    /// </summary>
    public const string SectionName = "Fusion:Auth0";

    /// <summary>
    /// Gets or sets the Auth0 Client ID.
    /// </summary>
    public required string ClientId { get; init; }

    /// <summary>
    /// Gets or sets the Auth0 Client Secret.
    /// </summary>
    public required string ClientSecret { get; init; }

    /// <summary>
    /// Gets or sets the Auth0 Domain.
    /// </summary>
    public required string Domain { get; init; }

    /// <summary>
    /// Gets or sets the Authority URL for the Auth0 authentication.
    /// </summary>
    public required string Authority { get; init; }

    /// <summary>
    /// Gets or sets the Audience for the Auth0 authentication.
    /// </summary>
    public required string Audience { get; init; }
}
