using ErrorOr;

namespace Fusion.Core.Auth;

public static class AuthErrors
{
    public static ErrorOr<UserRoleDto> ValidationFailed =>
        Error.Validation("User.Role.ValidationFailed", "Profile DTO validation failed");

    public static ErrorOr<UserRoleDto> ProfileMisconfigured =>
        Error.Validation("User.Profile.Misconfigured", "User's profile isn't configured properly");

    public static ErrorOr<UserRoleDto> NotFound(long id) =>
        Error.NotFound("User.Role.NotFound", $"Profile with ID( {id} ) not found");
}