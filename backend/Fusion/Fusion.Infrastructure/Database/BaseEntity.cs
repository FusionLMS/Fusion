using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fusion.Infrastructure.Database;

public class BaseEntity<TKey>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required TKey Id { get; init; }
}