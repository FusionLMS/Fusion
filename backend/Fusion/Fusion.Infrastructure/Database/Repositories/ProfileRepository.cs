using System.Diagnostics.CodeAnalysis;
using Fusion.Infrastructure.Database.Abstractions;
using Fusion.Infrastructure.Profile;

namespace Fusion.Infrastructure.Database.Repositories;

[ExcludeFromCodeCoverage]
public class ProfileRepository(ApplicationDbContext dbContext)
    : RepositoryBase<long, ProfileEntity, ApplicationDbContext>(dbContext), IProfileRepository
{
}