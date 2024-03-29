﻿using System.Diagnostics.CodeAnalysis;
using Asp.Versioning.Builder;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Fusion.Core.Profile;
using Fusion.Core.Auth;
using Fusion.Infrastructure.Auth.Options;
using Fusion.RestApi.Auth.Models;
using Fusion.RestApi.Extensions;
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

        authV1.MapPost("roles", async(
            UserRolesViewModel req,
            HttpContext httpContext,
            IAuthService authService,
            CancellationToken ct) =>
        {
            var dto = new UserRoleDto()
            {
                FusionUserId = req.UserId,
                Roles = req.Roles
            };

            var result = await authService.AssignUserToRole(dto, ct);
            return result.Match(Results.Ok, e => e.Problem(httpContext));
        });

        authV1.MapPost("roles/unassign", async(
            UserRolesViewModel req,
            HttpContext httpContext,
            IAuthService authService,
            CancellationToken ct) =>
        {
            var dto = new UserRoleDto
            {
                FusionUserId = req.UserId,
                Roles = req.Roles
            };

            var result = await authService.UnAssignUserFromRole(dto, ct);
            return result.Match(Results.Ok, e => e.Problem(httpContext));
        });
        
        authV1.MapPost("postLoginHandler", async (PostLoginHandleModel req, HttpContext httpContext, IProfileService profileService, CancellationToken ct) =>
        {
            ArgumentNullException.ThrowIfNull(req);
            var provider = req.Auth0UserId.Split('|')[0];
            if(provider.Equals("google-oauth2") || provider.Equals("github"))
            {
                var result = await profileService.Create(new ProfileDto
                {
                    FirstName = req.FirstName,
                    LastName = req.LastName,
                    Email = req.Email,
                    Auth0UserId = req.Auth0UserId
                });
                return result.Match(Results.Ok, e => e.Problem(context: httpContext));
            }
            
            return Results.Ok(); 
        });
    }
}