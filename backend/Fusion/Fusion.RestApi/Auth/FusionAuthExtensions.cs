using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Fusion.RestApi.Auth;

internal static class FusionAuthExtensions
{
    public static IServiceCollection AddFusionAuthentication(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.Authority = configuration["Fusion:Auth0:Authority"];
                opt.Audience = configuration["Fusion:Auth0:Audience"];
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    NameClaimType = ClaimTypes.NameIdentifier,
                    ValidAudience = configuration["Fusion:Auth0:Audience"],
                    ValidIssuer = configuration["Fusion:Auth0:Domain"]
                };
            });

        return services;
    }

    internal static void AddFusionAuthorization(
        this IServiceCollection services)
    {
        services
            .AddAuthorizationBuilder()
            .AddPolicy("access:full", policy => policy
                .RequireAuthenticatedUser()
                .RequireClaim("permissions", "access:full"));
    }
}