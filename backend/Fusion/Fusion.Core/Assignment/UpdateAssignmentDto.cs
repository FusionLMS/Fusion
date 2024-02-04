using Fusion.Infrastructure.Assignment;

namespace Fusion.Core.Assignment
{
    public record UpdateAssignmentDto
    {
        public required long Id { get; init; }

        public string? Title { get; init; }

        public string? Description { get; init; }

        public DateTime? StartDate { get; init; }

        public DateTime? Deadline { get; init; }

        public AssignmentType? Type { get; init; }

        public double? MaxGrade { get; init; }
    }
}
