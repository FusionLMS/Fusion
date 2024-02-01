using Fusion.Core.Profile;
using Microsoft.Extensions.DependencyInjection;

namespace Fusion.Core;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddFusionCore(this IServiceCollection services)
    {
        services.AddScoped<IProfileService, ProfileService>();

        return services;
    }
}