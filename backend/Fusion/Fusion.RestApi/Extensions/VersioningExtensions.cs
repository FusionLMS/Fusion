using Asp.Versioning;

namespace Fusion.RestApi.Extensions;

/// <summary>
/// 
/// </summary>
public static class VersioningExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddFusionApiVersioning(
        this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
        {
            // reporting API versions will return the headers
            // "api-supported-versions" and "api-deprecated-versions"
            opt.ReportApiVersions = true;
            opt.AssumeDefaultVersionWhenUnspecified = true;

            opt.DefaultApiVersion = new ApiVersion(0);

            // allows to pass API version via:
            // "api-version" header
            // "api-version" query parameter
            // "Content-Type" header
            opt.ApiVersionReader = ApiVersionReader.Combine([
                new HeaderApiVersionReader(),
                new QueryStringApiVersionReader(),
                new MediaTypeApiVersionReader()
            ]);
        })
        .AddApiExplorer(opt =>
        {
            // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            opt.GroupNameFormat = "'v'VVV";
        });

        return services;
    }
}