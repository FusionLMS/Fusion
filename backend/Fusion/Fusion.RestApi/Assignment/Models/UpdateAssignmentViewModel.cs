using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Fusion.Infrastructure.Assignment;

namespace Fusion.RestApi.Assignment.Models
{
    /// <summary>
    /// Model to edit assignment.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UpdateAssignmentViewModel
    {
        /// <summary>
        /// Title of assignment to update. If null title won't be updated.
        /// </summary>
        [MaxLength(128)]
        public string? Title { get; set; }

        /// <summary>
        /// Description of assignment to update. If null description won't be updated.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Start date of assignment to update. If null start date won't be updated.
        /// </summary>
        public DateTime? StartDate { get; init; }

        /// <summary>
        /// Deadline of assignment to update. If null deadline won't be updated.
        /// </summary>
        public DateTime? Deadline { get; init; }

        /// <summary>
        /// Type of assignment to update. If null type won't be updated.
        /// </summary>
        public AssignmentType? Type { get; init; }

        /// <summary>
        /// Max grade of assignment to update. If null max grade won't be updated.
        /// </summary>
        public double? MaxGrade { get; init; }
    }
}
