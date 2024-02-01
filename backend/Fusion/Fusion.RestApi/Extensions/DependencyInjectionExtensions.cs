using System.Diagnostics.CodeAnalysis;
using Fusion.Infrastructure.Database;
using Fusion.RestApi.Auth.Options;
using Fusion.RestApi.Swagger.Options;
using Microsoft.EntityFrameworkCore;

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

    public static IServiceCollection AddFusionDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseNpgsql(configuration["Fusion:Database:ConnectionString"]);
        });

        return services;
    }

}