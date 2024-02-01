using System.Diagnostics.CodeAnalysis;

namespace Fusion.RestApi.Auth.Models;

/// <summary>
/// Represents the data model for user sign-in.
/// </summary>
[ExcludeFromCodeCoverage]
public record SignInViewModel
{
    /// <summary>
    /// Gets the user's login identifier. This is typically a username or email address.
    /// </summary>
    /// <value>
    /// The login identifier for the user.
    /// </value>
    public required string Login { get; init; }

    /// <summary>
    /// Gets the user's password.
    /// </summary>
    /// <value>
    /// The password associated with the user's account.
    /// </value>
    public required string Password { get; init; }
}
