using ErrorOr;

namespace Fusion.Core.Profile;

public static class ProfileErrors
{
    public static ErrorOr<ProfileDto> Duplicate =>
        Error.Conflict("Profile.Duplicate", "Profile with same email already exists");
}