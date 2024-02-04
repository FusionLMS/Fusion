using System.Diagnostics.CodeAnalysis;
using Asp.Versioning.Builder;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Fusion.Infrastructure.Auth.Options;
using Fusion.RestApi.Auth.Models;
using Microsoft.Extensions.Options;

namespace Fusion.RestApi.Auth;

[ExcludeFromCodeCoverage]
internal static class AuthModule
{
    internal static void AddAuthEndpoints(this IVersionedEndpointRouteBuilder app)
    {
        var authV1 = app.MapGroup("/api/auth/")
            .WithTags("auth")
            .HasApiVersion(1);

        authV1.MapPost("login", async (
            SignInViewModel req,
            IOptions<Auth0Options> auth0Options,
            IAuthenticationApiClient authenticationApiClient,
            CancellationToken ct) =>
        {
            ArgumentNullException.ThrowIfNull(auth0Options.Value);
            var auth0Info = auth0Options.Value;

            var body = new ResourceOwnerTokenRequest
            {
                Username = req.Login,
                Password = req.Password,
                ClientId = auth0Info.ClientId,
                Audience = auth0Info.Audience,
                ClientSecret = auth0Info.ClientSecret,
                Scope = "openid"
            };
            var response = await authenticationApiClient.GetTokenAsync(body, ct);
            return Results.Ok(response);
        });
    }
}