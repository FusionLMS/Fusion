using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fusion.Infrastructure.Profile;

public class ProfileEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; init; }

    [MaxLength(64)]
    public required string FirstName { get; set; }

    [MaxLength(64)]
    public required string LastName { get; set; } 

    [MaxLength(32)]
    public required string Email { get; set; }
}