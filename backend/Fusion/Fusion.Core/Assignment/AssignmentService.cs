using ErrorOr;
using Fusion.Infrastructure.Assignment;
using Fusion.Infrastructure.Database.Abstractions;

namespace Fusion.Core.Assignment
{
    public interface IAssignmentService
    {
        Task<ErrorOr<AssignmentDto>> GetAsync(long assignmentId);
        Task<ErrorOr<AssignmentDto>> CreateAsync(CreateAssignmentDto assignmentToCreate);
        Task<ErrorOr<AssignmentDto>> UpdateAsync(UpdateAssignmentDto assignmentToUpdate);
        Task DeleteAsync(long assignmentId);
    }

    public class AssignmentService(IAssignmentRepository assignmentRepository)
        : IAssignmentService
    {
        public async Task<ErrorOr<AssignmentDto>> GetAsync(long assignmentId)
        {
            var assignment = await assignmentRepository.GetById(assignmentId);

            if (assignment is null)
            {
                return AssignmentErrors.NotFound(assignmentId);
            }

            var assignmentDto = new AssignmentDto
            {
                Id = assignment.Id,
                Title = assignment.Title,
                Description = assignment.Description,
                Deadline = assignment.Deadline,
                MaxGrade = assignment.MaxGrade,
                StartDate = assignment.StartDate,
                Status = assignment.Status,
                Type = assignment.Type,
            };

            return assignmentDto;
        }

        public async Task<ErrorOr<AssignmentDto>> CreateAsync(CreateAssignmentDto assignmentToCreate)
        {
            if (assignmentToCreate.StartDate.HasValue &&
                assignmentToCreate.Deadline < assignmentToCreate.StartDate.Value)
            {
                return AssignmentErrors.ValidationFailed;
            }

            var assignmentSpec = AssignmentSpecs.ByTitle(assignmentToCreate.Title);
            var isDuplicate = await assignmentRepository.ExistsBySpecification(assignmentSpec);
            if (isDuplicate)
            {
                return AssignmentErrors.Duplicate(assignmentToCreate.Title);
            }

            var assignmentEntityToCreate = new AssignmentEntity
            {
                Title = assignmentToCreate.Title,
                Description = assignmentToCreate.Description,
                Deadline = assignmentToCreate.Deadline,
                MaxGrade = assignmentToCreate.MaxGrade,
                StartDate = assignmentToCreate.StartDate.GetValueOrDefault(DateTime.UtcNow),
                Status = assignmentToCreate.StartDate.HasValue ? AssignmentStatus.Created : AssignmentStatus.Open,
                Type = assignmentToCreate.Type,
            };

            assignmentEntityToCreate = await assignmentRepository.Create(assignmentEntityToCreate);

            var assignmentDto = new AssignmentDto
            {
                Id = assignmentEntityToCreate.Id,
                Title = assignmentEntityToCreate.Title,
                Description = assignmentEntityToCreate.Description,
                Deadline = assignmentEntityToCreate.Deadline,
                MaxGrade = assignmentEntityToCreate.MaxGrade,
                StartDate = assignmentEntityToCreate.StartDate,
                Status = assignmentEntityToCreate.Status,
                Type = assignmentEntityToCreate.Type,
            };

            return assignmentDto;
        }

        public async Task<ErrorOr<AssignmentDto>> UpdateAsync(UpdateAssignmentDto assignmentToUpdate)
        {
            var assignment = await assignmentRepository.GetById(assignmentToUpdate.Id);

            if (assignment is null)
            {
                return AssignmentErrors.NotFound(assignmentToUpdate.Id);
            }

            assignment.Title = assignmentToUpdate.Title;
            assignment.Description = assignmentToUpdate.Description;
            assignment.StartDate = assignmentToUpdate.StartDate.GetValueOrDefault(assignment.StartDate);
            assignment.Deadline = assignmentToUpdate.Deadline;
            assignment.MaxGrade = assignmentToUpdate.MaxGrade;
            assignment.Type = assignmentToUpdate.Type;

            await assignmentRepository.Update(assignment.Id, assignment);

            var assignmentDto = new AssignmentDto
            {
                Id = assignment.Id,
                Title = assignment.Title,
                Description = assignment.Description,
                Deadline = assignment.Deadline,
                MaxGrade = assignment.MaxGrade,
                StartDate = assignment.StartDate,
                Status = assignment.Status,
                Type = assignment.Type,
            };

            return assignmentDto; 
        }

        public async Task DeleteAsync(long assignmentId)
        {
            await assignmentRepository.Delete(assignmentId);
        }
    }
}
