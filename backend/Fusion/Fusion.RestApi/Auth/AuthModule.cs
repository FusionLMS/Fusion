using System.Diagnostics.CodeAnalysis;
using Asp.Versioning.Builder;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Fusion.RestApi.Auth.Models;
using Fusion.RestApi.Auth.Options;
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

        authV1.MapPost("login", async (SignInViewModel req, IOptions<Auth0Options> auth0Options, CancellationToken ct) =>
        {
            ArgumentNullException.ThrowIfNull(auth0Options.Value);

            var auth0Info = auth0Options.Value;
            var auth0Client = new AuthenticationApiClient(auth0Info.Domain);

            return await auth0Client.GetTokenAsync(new ResourceOwnerTokenRequest
            {
                Username = req.Login,
                Password = req.Password,
                ClientId = auth0Info.ClientId,
                Audience = auth0Info.Audience,
                ClientSecret = auth0Info.ClientSecret,
                Scope = "openid"
            }, ct);
        });

        authV1.MapPost("callback", async (object obj) =>
        {
            ArgumentNullException.ThrowIfNull(obj);
        });
        
        authV1.MapPost("register", async (SignUpViewModel req, IOptions<Auth0Options> auth0Options, CancellationToken ct) =>
        {
            ArgumentNullException.ThrowIfNull(auth0Options.Value);

            var auth0Info = auth0Options.Value;
            var auth0Client = new AuthenticationApiClient(auth0Info.Domain);
            
            return await auth0Client.SignupUserAsync(new SignupUserRequest
            {
                ClientId = auth0Info.ClientId,
                Email = req.Email,
                Password = req.Password,
                GivenName = req.FirstName,
                FamilyName = req.LastName,
            }, cancellationToken: ct);
        });
    }
}