using Fusion.RestApi.OpenApi;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fusion.RestApi.Extensions;

/// <summary>
/// Set of swagger extensions methods
/// </summary>
public static class SwaggerExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddFusionSwaggerGen(
        this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(opt =>
        {
            // add a custom operation filter which sets default values
            opt.OperationFilter<SwaggerDefaultValues>();

            var fileName = typeof(Program).Assembly.GetName().Name + ".xml";
            var xmlCommentsFilePath = Path.Combine(AppContext.BaseDirectory, fileName);

            // integrate xml comments
            opt.IncludeXmlComments(xmlCommentsFilePath);
        });

        return services;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseFusionSwaggerUi(
        this IApplicationBuilder app)
    {
        app.UseSwagger()
            .UseSwaggerUI(opt =>
            {
                var descriptions = (app as IEndpointRouteBuilder)!.DescribeApiVersions();

                // build a swagger endpoint for each discovered API version
                foreach (var description in descriptions)
                {
                    var url = $"/swagger/{description.GroupName}/swagger.json";
                    var name = description.GroupName.ToUpperInvariant();
                    opt.SwaggerEndpoint(url, name);
                }
            });

        return app;
    }
}