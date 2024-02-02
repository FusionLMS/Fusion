using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fusion.RestApi.Profile.Models;

/// <summary>
/// 
/// </summary>
[ExcludeFromCodeCoverage]
public class ProfileViewModel
{
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [MaxLength(64)]
    public required string FirstName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [MaxLength(64)]
    public required string LastName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [MaxLength(32)]
    public required string Email { get; set; }
}