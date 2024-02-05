using System.Diagnostics.CodeAnalysis;
using Auth0Net.DependencyInjection;
using Fusion.Infrastructure.Auth.Options;
using Fusion.Infrastructure.Database.Abstractions;
using Fusion.Infrastructure.Database.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fusion.Infrastructure;

[ExcludeFromCodeCoverage]
public static class DependencyInjectionExtension
{
    public static IServiceCollection AddFusionInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IProfileRepository, ProfileRepository>();

        services.AddAuth0Services(configuration);

        return services;
    }

    private static void AddAuth0Services(
        this IServiceCollection services, IConfiguration configuration)
    {
        const string auth0SectionName = "Fusion:Auth0";

        services.Configure<Auth0Options>(
            configuration.GetSection(auth0SectionName));

        services
            .AddAuth0AuthenticationClient(opt =>
            {
                opt.Domain = configuration[auth0SectionName + ":Domain"]!;
                opt.ClientId = configuration[auth0SectionName + ":ClientId"];
                opt.ClientSecret = configuration[auth0SectionName + ":ClientSecret"];
            });

        services
            .AddAuth0ManagementClient()
            .AddManagementAccessToken();
    }
}