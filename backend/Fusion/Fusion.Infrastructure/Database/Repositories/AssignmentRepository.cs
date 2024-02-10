using Fusion.Infrastructure.Assignment;
using Fusion.Infrastructure.Database.Abstractions;

namespace Fusion.Infrastructure.Database.Repositories
{
    public class AssignmentRepository(ApplicationDbContext dbContext)
        : RepositoryBase<long, AssignmentEntity, ApplicationDbContext>(dbContext), IAssignmentRepository
    {
    }
}
