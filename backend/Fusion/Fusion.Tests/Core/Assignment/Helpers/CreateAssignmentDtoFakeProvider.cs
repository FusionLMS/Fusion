using Bogus;
using Fusion.Tests.Helpers;
using Fusion.Core.Assignment;
using Fusion.Infrastructure.Assignment;

namespace Fusion.Tests.Core.Assignment.Helpers
{
    public class CreateAssignmentDtoFakeProvider() : ObjectFakeProviderBase<CreateAssignmentDto>(5)
    {
        protected override Func<Faker<CreateAssignmentDto>> DefaultFactory =>
            () => new Faker<CreateAssignmentDto>()
                .RuleFor(x => x.Title, f => f.Name.JobTitle())
                .RuleFor(x => x.Description, f => f.Name.JobDescriptor())
                .RuleFor(x => x.StartDate, f => f.Date.Future())
                .RuleFor(x => x.Deadline, (_, a) => a.StartDate!.Value.AddDays(5))
                .RuleFor(x => x.Type, f => f.PickRandom<AssignmentType>())
                .RuleFor(x => x.MaxGrade, f => f.Random.Double(10, 100));
    }
}
