using System.Diagnostics.CodeAnalysis;
using Fusion.RestApi.Auth.Options;
using Fusion.RestApi.Swagger.Options;

namespace Fusion.RestApi.Extensions;

[ExcludeFromCodeCoverage]
internal static class DependencyInjectionExtensions
{
    public static IServiceCollection AddFusionOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SwaggerContactsInfoOptions>(
            configuration.GetSection(SwaggerContactsInfoOptions.SectionName));

        services.Configure<Auth0Options>(
            configuration.GetSection(Auth0Options.SectionName));

        return services;
    }
}