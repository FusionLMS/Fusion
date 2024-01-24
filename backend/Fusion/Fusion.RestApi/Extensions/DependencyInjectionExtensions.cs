using System.Diagnostics.CodeAnalysis;
using Fusion.RestApi.OpenApi.Options;

namespace Fusion.RestApi.Extensions;

[ExcludeFromCodeCoverage]
internal static class DependencyInjectionExtensions
{
    public static IServiceCollection AddFusionOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SwaggerContactsInfoOptions>(
            configuration.GetSection(SwaggerContactsInfoOptions.SectionName));

        return services;
    }
}