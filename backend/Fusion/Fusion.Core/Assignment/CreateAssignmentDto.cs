using Fusion.Infrastructure.Assignment;

namespace Fusion.Core.Assignment
{
    public record CreateAssignmentDto
    {
        public required string Title { get; init; }

        public required string Description { get; init; }

        public DateTime? StartDate { get; init; }

        public DateTime Deadline { get; init; }

        public AssignmentType Type { get; init; }

        public double MaxGrade { get; init; }
    }
}
