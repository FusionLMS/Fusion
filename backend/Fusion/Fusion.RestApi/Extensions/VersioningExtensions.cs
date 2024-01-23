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
        services.AddApiVersioning(
                options =>
                {
                    // reporting api versions will return the headers
                    // "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;

                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(0);
                })
            .AddApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                })
            // this enables binding ApiVersion as a endpoint callback parameter. if you don't use it, then
            // you should remove this configuration.
            .EnableApiVersionBinding();

        return services;
    }
}