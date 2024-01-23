using System.Diagnostics.CodeAnalysis;
using Fusion.RestApi.Extensions;
using Fusion.RestApi.Routing;

var builder = WebApplication.CreateBuilder(args);
AddFusionServices(builder);

var app = builder.Build();
ConfigureMiddlewares(app);

app.Run();
return;


void AddFusionServices(IHostApplicationBuilder webApplicationBuilder)
{
    var services = webApplicationBuilder.Services;

    services
        .AddProblemDetails()
        .AddHealthChecks();

    services
        .AddFusionApiVersioning()
        .AddFusionSwaggerGen();
}

void ConfigureMiddlewares(WebApplication appBuilder)
{
    appBuilder.MapFusionRouting();
    appBuilder.UseFusionSwaggerUi();
}

[ExcludeFromCodeCoverage]
internal partial class Program;