using System.ComponentModel.DataAnnotations;
using Fusion.Infrastructure.Database;

namespace Fusion.Infrastructure.Profile;

public class ProfileEntity : BaseEntity<long>
{
    [Required]
    [MaxLength(64)]
    public required string FirstName { get; set; }

    [Required]
    [MaxLength(64)]
    public required string LastName { get; set; } 

    [Required]
    [MaxLength(32)]
    public required string Email { get; set; }
}