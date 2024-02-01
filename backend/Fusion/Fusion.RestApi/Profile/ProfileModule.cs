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
            ProfileViewModel req,
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

        profileV1.MapGet("{id:long}", async (
            long id,
            HttpContext httpContext,
            IProfileService profileService,
            CancellationToken _) =>
        {
            var result = await profileService.Get(id);
            return result.Match(Results.Ok, e => e.Problem(context: httpContext));
        });

        profileV1.MapPatch("{id:long}", async (
            long id,
            ProfileViewModel req,
            HttpContext httpContext,
            IProfileService profileService,
            CancellationToken _) =>
        {
            var profileDto = new ProfileDto
            {
                Id = id,
                FirstName = req.FirstName,
                LastName = req.LastName,
                Email = req.Email
            };
 
            var result = await profileService.Update(profileDto);
            return result.Match(Results.Ok, e => e.Problem(context: httpContext));
        });

        profileV1.MapDelete("{id:long}", async (
            long id,
            IProfileService profileService,
            CancellationToken _) =>
        {
            await profileService.Delete(id);
            return Results.Ok();
        });
    }
}