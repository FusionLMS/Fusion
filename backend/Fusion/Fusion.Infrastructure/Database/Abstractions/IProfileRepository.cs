using Fusion.Infrastructure.Profile;

namespace Fusion.Infrastructure.Database.Abstractions;

public interface IProfileRepository : IRepository<long, ProfileEntity>
{
}