using Bogus;
using Fusion.Core.Assignment;
using Fusion.Infrastructure.Assignment;
using Fusion.Tests.Helpers;

namespace Fusion.Tests.Core.Assignment.Helpers
{
    public class AssignmentDtoFakeProvider() : ObjectFakeProviderBase<AssignmentDto>(5)
    {
        protected override Func<Faker<AssignmentDto>> DefaultFactory =>
            () => new Faker<AssignmentDto>()
                .RuleFor(x => x.Id, f => f.IndexFaker)
                .RuleFor(x => x.Title, f => f.Name.JobTitle())
                .RuleFor(x => x.Description, f => f.Name.JobDescriptor())
                .RuleFor(x => x.StartDate, f => f.Date.Future())
                .RuleFor(x => x.Deadline, (_, a) => a.StartDate.AddDays(5))
                .RuleFor(x => x.Type, f => f.PickRandom<AssignmentType>())
                .RuleFor(x => x.MaxGrade, f => f.Random.Double(10, 100))
                .RuleFor(x => x.Status, f => f.PickRandom<AssignmentStatus>());
    }
}
