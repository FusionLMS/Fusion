using System.Diagnostics.CodeAnalysis;

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
        var v1 = routeBuilder.NewVersionedApi();

        var serviceHealthV1 = v1.MapGroup("/api/health/")
            .HasApiVersion(1.0);

        serviceHealthV1.MapHealthChecks("healthz");
        serviceHealthV1.MapGet("status", () => Results.Ok("Ok"));
    }
}