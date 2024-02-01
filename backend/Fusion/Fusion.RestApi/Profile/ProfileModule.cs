using Asp.Versioning.Builder;
using Fusion.Core.Profile;
using Fusion.RestApi.Extensions;
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
            .HasApiVersion(1)
            .RequireAuthorization("backend-developer");

        profileV1.MapPost("", async (
            HttpContext httpContext,
            CreateProfileViewModel req,
            IProfileService profileService,
            CancellationToken _) =>
        {
            var profileDto = new ProfileDto
            {
                FirstName = req.FirstName,
                LastName = req.LastName,
                Email = req.Email
            };
 
            var result = await profileService.Create(profileDto);
            return result.Match(Results.Ok, e => e.Problem(context: httpContext));
        });
    }
}