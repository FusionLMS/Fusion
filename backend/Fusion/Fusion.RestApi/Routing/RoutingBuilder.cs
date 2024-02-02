using System.Diagnostics.CodeAnalysis;
using Fusion.RestApi.Auth;
using Fusion.RestApi.Profile;

namespace Fusion.RestApi.Routing;

/// <summary>
/// 
/// </summary>
[ExcludeFromCodeCoverage]
public static class RoutingBuilder
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="routeBuilder"></param>
    public static void MapFusionRouting(this IEndpointRouteBuilder routeBuilder)
    {
        var app = routeBuilder.NewVersionedApi();

        var serviceHealthV1 = app.MapGroup("/api/health/")
            .WithTags("health")
            .HasApiVersion(1);

        serviceHealthV1.MapHealthChecks("healthz");
        serviceHealthV1.MapGet("status", () => Results.Ok("Ok"))
            .RequireAuthorization("backend-developer");

        app.AddAuthEndpoints();
        app.AddProfileEndpoints();
    }
}