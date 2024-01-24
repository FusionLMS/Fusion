using System.Diagnostics.CodeAnalysis;
using System.Text;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Fusion.RestApi.OpenApi.Options;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fusion.RestApi.OpenApi;

/// <summary>
/// 
/// </summary>
[ExcludeFromCodeCoverage]
public class ConfigureSwaggerOptions: IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    private readonly SwaggerContactsInfoOptions _contactInfo;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="contactOptions"></param>
    public ConfigureSwaggerOptions(
        IApiVersionDescriptionProvider provider,
        IOptions<SwaggerContactsInfoOptions> contactOptions)
    {
        _provider = provider;
        ArgumentNullException.ThrowIfNull(contactOptions.Value);
        _contactInfo = contactOptions.Value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }

    private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var text = new StringBuilder($"Fusion LMS API v{description.ApiVersion}.");
        var info = new OpenApiInfo()
        {
            Title = "Fusion LMS API",
            Version = description.ApiVersion.ToString(),
            Contact = new OpenApiContact { Name = "Fusion LMS Team", Url = new Uri(_contactInfo.ContactUri!) },
            License = new OpenApiLicense { Name = "MIT", Url = new Uri(_contactInfo.LicenseUri!) }
        };

        if (description.IsDeprecated)
        {
            text.Append(" This API version has been deprecated.");
        }

        if (description.SunsetPolicy is { } policy)
        {
            GetApiVersionSunsetInfo(policy, text);
        }

        info.Description = text.ToString();

        return info;
    }

    private static void GetApiVersionSunsetInfo(SunsetPolicy policy, StringBuilder text)
    {
        if (policy.Date is { } when)
        {
            text.Append(" The API will be sunset on ")
                .Append(when.Date.ToShortDateString())
                .Append('.');
        }

        if (!policy.HasLinks)
        {
            return;
        }

        text.AppendLine();

        foreach (var link in policy.Links)
        {
            GetLinkInfo(text, link);
        }
    }

    private static void GetLinkInfo(StringBuilder text, LinkHeaderValue link)
    {
        if (link.Type != "text/html")
        {
            return;
        }

        text.AppendLine();

        if (link.Title.HasValue)
        {
            text.Append(link.Title.Value).Append(": ");
        }

        text.Append(link.LinkTarget.OriginalString);
    }
}