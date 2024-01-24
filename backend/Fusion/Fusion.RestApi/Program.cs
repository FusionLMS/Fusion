using System.Diagnostics.CodeAnalysis;
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
            .AddProblemDetails()
            .AddHealthChecks();

        services
            .AddFusionApiVersioning()
            .AddFusionSwaggerGen()
            .AddFusionOptions(appBuilder.Configuration);
    }

    private static void ConfigureMiddlewares(WebApplication appBuilder)
    {
        appBuilder.MapFusionRouting();
        appBuilder.UseFusionSwaggerUi();
    }
}