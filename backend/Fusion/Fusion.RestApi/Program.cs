using Fusion.RestApi.Extensions;
using Fusion.RestApi.Routing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();

builder.Services
    .AddFusionApiVersioning()
    .AddFusionSwaggerGen();


var app = builder.Build();

var versionedRouteBuilder = app
    .NewVersionedApi()
    .HasApiVersion(0);

app.UseFusionSwaggerUi();
versionedRouteBuilder.MapFusionRouting();

app.Run();
