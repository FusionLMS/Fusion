using System.Diagnostics.CodeAnalysis;
using Fusion.Infrastructure.Profile;
using Microsoft.EntityFrameworkCore;

namespace Fusion.Infrastructure.Database;

[ExcludeFromCodeCoverage]
public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<ProfileEntity> Profiles { get; set; } = null!;
}