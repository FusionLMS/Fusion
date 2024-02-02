using System.Diagnostics.CodeAnalysis;
using Fusion.Core;
using Fusion.Infrastructure;
using Fusion.RestApi.Auth;
using Fusion.RestApi.Extensions;
using Fusion.RestApi.Routing;

namespace Fusion.RestApi;

/// <summary>
/// Application entry point
/// </summary>
[ExcludeFromCodeCoverage]
public static class Program
{
    internal static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        AddFusionServices(builder);

        var app = builder.Build();
        
        ConfigureMiddlewares(app);

        app.Run();
    }

    private static void AddFusionServices(IHostApplicationBuilder appBuilder)
    {
        var services = appBuilder.Services;

        services
            .AddFusionAuthentication(appBuilder.Configuration)
            .AddFusionAuthorization();

        services
            .AddFusionDatabase(appBuilder.Configuration);

        services
            .AddProblemDetails()
            .AddHealthChecks();

        services
            .AddFusionApiVersioning()
            .AddFusionSwaggerGen()
            .AddFusionOptions(appBuilder.Configuration);

        services
            .AddFusionInfrastructure()
            .AddFusionCore();
    }

    private static void ConfigureMiddlewares(WebApplication appBuilder)
    {
        if (appBuilder.Environment.IsDevelopment())
        {
            appBuilder.UseDeveloperExceptionPage();
            appBuilder.ApplyMigrations();
        }

        appBuilder.UseAuthentication();
        appBuilder.UseAuthorization();

        appBuilder.MapFusionRouting();
        appBuilder.UseFusionSwaggerUi();
    }
}