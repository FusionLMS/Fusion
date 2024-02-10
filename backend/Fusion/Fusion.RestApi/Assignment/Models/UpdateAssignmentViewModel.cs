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
        [Required]
        [MaxLength(128)]
        public required string Title { get; set; }

        /// <summary>
        /// Description of assignment to update. If null description won't be updated.
        /// </summary>
        [Required]
        public required string Description { get; set; }

        /// <summary>
        /// Start date of assignment to update. If null start date won't be updated.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Deadline of assignment to update. If null deadline won't be updated.
        /// </summary>
        [Required]
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Type of assignment to update. If null type won't be updated.
        /// </summary>
        [Required]
        public AssignmentType Type { get; set; }

        /// <summary>
        /// Max grade of assignment to update. If null max grade won't be updated.
        /// </summary>
        [Required]
        public double MaxGrade { get; set; }
    }
}
