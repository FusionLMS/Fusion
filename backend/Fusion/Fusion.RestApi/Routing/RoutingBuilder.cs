namespace Fusion.RestApi.Routing;

/// <summary>
/// 
/// </summary>
public static class RoutingBuilder
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="routeBuilder"></param>
    public static void MapFusionRouting(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder
            .MapHealthChecks("healthz")
            .MapToApiVersion(0);
    }
}