using System.Diagnostics.CodeAnalysis;

namespace Fusion.RestApi.Auth.Models;

/// <summary>
/// Represents the data model in post register flow.
/// </summary>
[ExcludeFromCodeCoverage]
public record PostRegisterHandleModel
{
    /// <summary>
    /// Gets the user's first(given) name.
    /// </summary>
    public string FirstName { get; init; }
    /// <summary>
    /// Gets the user's last(family) name.
    /// </summary>
    public string LastName { get; init; }
    /// <summary>
    /// Gets the user's email address.
    /// </summary>
    public string Email { get; init; }
    /// <summary>
    /// Gets the user's Auth0 user id.
    /// </summary>
    public string Auth0UserId { get; init; }
};