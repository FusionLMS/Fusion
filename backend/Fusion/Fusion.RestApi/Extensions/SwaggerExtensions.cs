using System.Diagnostics.CodeAnalysis;
using Fusion.RestApi.OpenApi;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fusion.RestApi.Extensions;

/// <summary>
/// Set of swagger extensions methods
/// </summary>
[ExcludeFromCodeCoverage]
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
        services.AddEndpointsApiExplorer();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(options =>
        {
            options.OperationFilter<SwaggerDefaultValues>();

            var fileName = typeof(Program).Assembly.GetName().Name + ".xml";
            var xmlCommentsFilePath = Path.Combine(AppContext.BaseDirectory, fileName);

            // integrate xml comments
            options.IncludeXmlComments(xmlCommentsFilePath);

            options.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    }, []
                }
            });
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
                var groupNames = descriptions.Select(x => x.GroupName);
                foreach (var groupName in groupNames)
                {
                    var url = $"/swagger/{groupName}/swagger.json";
                    var name = groupName.ToUpperInvariant();
                    opt.SwaggerEndpoint(url, name);
                }
            });

        return app;
    }
}