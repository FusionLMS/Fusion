using Bogus;
using Fusion.Core.Assignment;
using Fusion.Tests.Helpers;
using Fusion.Infrastructure.Assignment;

namespace Fusion.Tests.Core.Assignment.Helpers
{
    public class UpdateAssignmentDtoFakeProvider() : ObjectFakeProviderBase<UpdateAssignmentDto>(5)
    {
        protected override Func<Faker<UpdateAssignmentDto>> DefaultFactory =>
            () => new Faker<UpdateAssignmentDto>()
                .RuleFor(x => x.Id, f => f.IndexFaker)
                .RuleFor(x => x.Title, f => f.Name.JobTitle())
                .RuleFor(x => x.Description, f => f.Name.JobDescriptor())
                .RuleFor(x => x.StartDate, f => f.Date.Future().OrNull(f))
                .RuleFor(x => x.Deadline, (_, a) => a.StartDate.GetValueOrDefault(DateTime.UtcNow).AddDays(5))
                .RuleFor(x => x.Type, f => f.PickRandom<AssignmentType>())
                .RuleFor(x => x.MaxGrade, f => f.Random.Double(10, 100));
    }
}
