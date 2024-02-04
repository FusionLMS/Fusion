using System.ComponentModel.DataAnnotations;
using Fusion.Infrastructure.Database;

namespace Fusion.Infrastructure.Assignment
{
    public class AssignmentEntity : BaseEntity<long>
    {
        [Required]
        [MaxLength(128)]
        public required string Title { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        public AssignmentType Type { get; set; }

        [Required] 
        public double MaxGrade { get; set; }

        public AssignmentStatus Status { get; set; }
    }
}
