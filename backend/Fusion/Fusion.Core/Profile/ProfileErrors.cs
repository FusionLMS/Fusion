using ErrorOr;

namespace Fusion.Core.Profile;

public static class ProfileErrors
{
    public static ErrorOr<ProfileDto> ValidationFailed =>
        Error.Validation("Profile.ValidationFailed", "Profile DTO validation failed");

    public static ErrorOr<ProfileDto> Duplicate(string email) =>
        Error.Conflict("Profile.Duplicate", $"Profile with EMAIL( {email} ) already exists");

    public static ErrorOr<ProfileDto> NotFound(long id) =>
        Error.NotFound("Profile.NotFound", $"Profile with ID( {id} ) not found");
}