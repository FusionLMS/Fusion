namespace Fusion.RestApi.Auth.Models;

/// <summary>
/// Represents the data model for user sign-up.
/// </summary>
public record SignUpViewModel
{
    /// <summary>
    /// Gets the user's email.
    /// </summary>
    /// <value>
    /// Email of the user.
    /// </value>
    public required string Email { get; init; }

    /// <summary>
    /// Gets the user's first name.
    /// </summary>
    /// <value>
    /// First name of the user.
    /// </value>
    public required string FirstName { get; set; }
    /// <summary>
    /// Gets the user's last name.
    /// </summary>
    /// <value>
    /// Last name of the user.
    /// </value>
    public required string LastName { get; set; }
    /// <summary>
    /// Gets the user's password.
    /// </summary>
    /// <value>
    /// The password associated with the user's account.
    /// </value>
    public required string Password { get; init; }
};