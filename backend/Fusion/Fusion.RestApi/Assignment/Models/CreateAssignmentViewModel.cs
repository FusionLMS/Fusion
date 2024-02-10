using System.ComponentModel.DataAnnotations;
using Fusion.Infrastructure.Assignment;
using System.Diagnostics.CodeAnalysis;

namespace Fusion.RestApi.Assignment.Models
{
    /// <summary>
    /// Model to create assignment.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CreateAssignmentViewModel
    {
        /// <summary>
        /// Title of assignment. Cannot exceed 128 symbols.
        /// </summary>
        [Required]
        [MaxLength(128)]
        public required string Title { get; set; }

        /// <summary>
        /// Assignment description.
        /// </summary>
        [Required]
        public required string Description { get; set; }

        /// <summary>
        /// Assignment start date. If null start date will be now.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Assignment deadline.
        /// </summary>
        [Required]
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Assignment type.
        /// </summary>
        [Required]
        public AssignmentType Type { get; set; }

        /// <summary>
        /// Assignment max grade.
        /// </summary>
        [Required]
        public double MaxGrade { get; set; }
    }
}
