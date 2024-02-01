using System.Diagnostics.CodeAnalysis;
using Fusion.Infrastructure.Database.Abstractions;
using Fusion.Infrastructure.Database.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Fusion.Infrastructure;

[ExcludeFromCodeCoverage]
public static class DependencyInjectionExtension
{
    public static IServiceCollection AddFusionInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IProfileRepository, ProfileRepository>();

        return services;
    }
}