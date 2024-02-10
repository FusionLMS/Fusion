using System.Diagnostics.CodeAnalysis;
using Fusion.Core.Assignment;
using Fusion.Core.Profile;
using Microsoft.Extensions.DependencyInjection;

namespace Fusion.Core;

[ExcludeFromCodeCoverage]
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddFusionCore(this IServiceCollection services)
    {
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IAssignmentService, AssignmentService>();

        return services;
    }
}