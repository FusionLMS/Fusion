using Asp.Versioning.Builder;
using Fusion.Core.Profile;
using Fusion.RestApi.Profile.Models;

namespace Fusion.RestApi.Profile;

/// <summary>
/// 
/// </summary>
public static class ProfileModule
{
    internal static void AddProfileEndpoints(this IVersionedEndpointRouteBuilder app)
    {
        var profileV1 = app.MapGroup("/api/profiles/")
            .WithTags("Profile")
            .HasApiVersion(1);

        profileV1.MapPost("", async (CreateProfileViewModel req, IProfileService profileService, CancellationToken ct) =>
        {
            var profileDto = new ProfileDto
            {
                FirstName = req.FirstName,
                LastName = req.LastName,
                Email = req.Email
            };

            return await profileService.Create(profileDto);
        });
    }
}