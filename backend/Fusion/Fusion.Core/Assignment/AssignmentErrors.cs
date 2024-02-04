using ErrorOr;

namespace Fusion.Core.Assignment
{
    public static class AssignmentErrors
    {
        public static ErrorOr<AssignmentDto> NotFound(long id) =>
            Error.NotFound("Assignment.NotFound", $"Assignment with ID( {id} ) not found");

        public static ErrorOr<AssignmentDto> ValidationFailed =>
            Error.Validation("Assignment.ValidationFailed", "Assignment DTO validation failed");

        public static ErrorOr<AssignmentDto> Duplicate(string title) =>
            Error.Conflict("Assignment.Duplicate", $"Assignment with TITLE( {title} ) already exists");
    }
}
