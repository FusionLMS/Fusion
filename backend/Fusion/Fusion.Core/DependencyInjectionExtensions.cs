using System.Diagnostics.CodeAnalysis;
using Fusion.Core.Auth;
using Fusion.Core.Profile;
using Microsoft.Extensions.DependencyInjection;

namespace Fusion.Core;

[ExcludeFromCodeCoverage]
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddFusionCore(this IServiceCollection services)
    {
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}