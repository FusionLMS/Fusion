using System.Diagnostics.CodeAnalysis;

namespace Fusion.RestApi.OpenApi.Options;

/// <summary>
/// Represents the contact information options used in Swagger documentation.
/// This class provides properties for configuring Swagger's contact and license links.
/// </summary>
[ExcludeFromCodeCoverage]
public class SwaggerContactsInfoOptions
{
    /// <summary>
    /// Gets the configuration section name in the application settings.
    /// Use this constant to access the Swagger Contacts configuration.
    /// </summary>
    public const string SectionName = "Fusion:Swagger:Contacts";

    /// <summary>
    /// Gets or sets the URI of the contact link. 
    /// This URI is displayed in the Swagger documentation to provide contact information.
    /// It can be null if no contact information is to be displayed.
    /// </summary>
    public string? ContactUri { get; set; }

    /// <summary>
    /// Gets or sets the URI of the license link.
    /// This URI is used to display licensing information in the Swagger documentation.
    /// It can be null if no licensing information is to be provided.
    /// </summary>
    public string? LicenseUri { get; set; }
}
