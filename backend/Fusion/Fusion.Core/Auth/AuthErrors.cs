using ErrorOr;

namespace Fusion.Core.Auth;

public static class AuthErrors
{
    public static ErrorOr<UserRoleDto> ProfileMisconfigured =>
        Error.Validation("User.Profile.Misconfigured", "User's profile isn't configured properly");
}