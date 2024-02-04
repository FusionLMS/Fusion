using FluentAssertions;
using Fusion.Core.Assignment;
using Fusion.Infrastructure.Assignment;
using Fusion.Infrastructure.Database.Abstractions;
using Fusion.Infrastructure.Database.Specifications;
using Fusion.Tests.Core.Assignment.Helpers;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace Fusion.Tests.Core.Assignment
{
    public class AssignmentServiceTests
    {
        private readonly IAssignmentRepository _assignmentRepositoryMock;
        private readonly IAssignmentService _sut;

        public AssignmentServiceTests()
        {
            _assignmentRepositoryMock = Substitute.For<IAssignmentRepository>();
            _sut = new AssignmentService(_assignmentRepositoryMock);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnNotFoundError_WhenNoAssignmentFound()
        {
            // Arrange
            long assignmentId = long.MaxValue - 1;
            _assignmentRepositoryMock
                .GetById(assignmentId)
                .ReturnsNull();

            // Act
            var result = await _sut.GetAsync(assignmentId);

            // Assert
            result.IsError.Should().BeTrue();
            result.FirstError.Should().BeEquivalentTo(AssignmentErrors.NotFound(assignmentId).FirstError);
        }

        [Theory]
        [ClassData(typeof(AssignmentDtoFakeProvider))]
        public async Task GetAsync_ShouldReturnAssignment_WhenAssignmentHasBeenFound(AssignmentDto assignment)
        {
            // Arrange
            var assignmentEntity = new AssignmentEntity
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
            _assignmentRepositoryMock
                .GetById(assignment.Id)!
                .Returns(Task.FromResult(assignmentEntity));

            // Act
            var result = await _sut.GetAsync(assignment.Id);

            // Assert
            result.Value.Should().NotBeNull();
            result.IsError.Should().BeFalse();
            result.Value.Id.Should().Be(assignment.Id);
            result.Value.Title.Should().Be(assignment.Title);
            result.Value.Description.Should().Be(assignment.Description);
            result.Value.Deadline.Should().Be(assignment.Deadline);
            result.Value.MaxGrade.Should().Be(assignment.MaxGrade);
            result.Value.StartDate.Should().Be(assignment.StartDate);
            result.Value.Status.Should().Be(assignment.Status);
            result.Value.Type.Should().Be(assignment.Type);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnValidationError_WhenStartDateIsLaterThanDeadline()
        {
            // Arrange
            var assignmentToCreate = new CreateAssignmentDto
            {
                Title = string.Empty,
                Description = string.Empty,
                Deadline = DateTime.MinValue,
                StartDate = DateTime.UtcNow,
            };

            // Act
            var result = await _sut.CreateAsync(assignmentToCreate);

            // Assert
            result.Value.Should().BeNull();
            result.IsError.Should().BeTrue();
            result.FirstError.Should().BeEquivalentTo(AssignmentErrors.ValidationFailed.FirstError);
        }

        [Theory]
        [ClassData(typeof(AssignmentDtoFakeProvider))]
        public async Task CreateAsync_ShouldReturnDuplicateError_WhenAssignmentTitleIsNotUnique(AssignmentDto assignmentDto)
        {
            // Arrange
            var assignmentToCreate = new CreateAssignmentDto
            {
                Title = assignmentDto.Title,
                Description = assignmentDto.Description,
                Deadline = assignmentDto.Deadline,
                StartDate = assignmentDto.StartDate,
            };
            _assignmentRepositoryMock
                .ExistsBySpecification(Arg.Any<Specification<AssignmentEntity>>())
                .Returns(true);

            // Act
            var result = await _sut.CreateAsync(assignmentToCreate);

            // Assert
            result.Value.Should().BeNull();
            result.IsError.Should().BeTrue();
            result.FirstError.Should().BeEquivalentTo(AssignmentErrors.Duplicate(assignmentDto.Title).FirstError);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateAssignmentImmediately_WhenAssignmentStartDateIsNull()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var assignmentToCreate = new CreateAssignmentDto
            {
                Title = string.Empty,
                Description = string.Empty,
                StartDate = null,
            };
            var assignmentEntity = new AssignmentEntity
            {
                Title = string.Empty,
                Description = string.Empty,
                Deadline = DateTime.MaxValue,
                StartDate = now,
                Status = AssignmentStatus.Open
            };
            _assignmentRepositoryMock
                .ExistsBySpecification(Arg.Any<Specification<AssignmentEntity>>())
                .Returns(false);
            _assignmentRepositoryMock
                .Create(Arg.Any<AssignmentEntity>())
                .Returns(Task.FromResult(assignmentEntity));

            // Act
            var result = await _sut.CreateAsync(assignmentToCreate);

            // Assert
            result.Value.Should().NotBeNull();
            result.IsError.Should().BeFalse();
            result.Value.StartDate.Should().Be(now);
            result.Value.Status.Should().Be(AssignmentStatus.Open);
        }

        [Theory]
        [ClassData(typeof(CreateAssignmentDtoFakeProvider))]
        public async Task CreateAsync_ShouldCreateAssignmentLately_WhenAssignmentStartDateIsSpecified(CreateAssignmentDto assignmentDto)
        {
            // Arrange
            var assignmentEntity = new AssignmentEntity
            {
                Id = 1,
                Title = assignmentDto.Title,
                Description = assignmentDto.Description,
                Deadline = assignmentDto.Deadline,
                StartDate = assignmentDto.StartDate!.Value,
                MaxGrade = assignmentDto.MaxGrade,
                Status = AssignmentStatus.Created
            };
            _assignmentRepositoryMock
                .ExistsBySpecification(Arg.Any<Specification<AssignmentEntity>>())
                .Returns(false);
            _assignmentRepositoryMock
                .Create(Arg.Any<AssignmentEntity>())
                .Returns(Task.FromResult(assignmentEntity));

            // Act
            var result = await _sut.CreateAsync(assignmentDto);

            // Assert
            result.Value.Should().NotBeNull();
            result.IsError.Should().BeFalse();
            result.Value.Title.Should().Be(assignmentDto.Title);
            result.Value.Description.Should().Be(assignmentDto.Description);
            result.Value.Deadline.Should().Be(assignmentDto.Deadline);
            result.Value.StartDate.Should().Be(assignmentDto.StartDate);
            result.Value.MaxGrade.Should().Be(assignmentDto.MaxGrade);
            result.Value.Status.Should().Be(AssignmentStatus.Created);
            result.Value.Id.Should().Be(assignmentEntity.Id);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNotFoundError_WhenAssignmentHasNotBeenFound()
        {
            // Arrange
            var assignmentToUpdate = new UpdateAssignmentDto
            {
                Id = 1, 
                Title = string.Empty,
                Description = string.Empty,
            };
            _assignmentRepositoryMock
                .GetById(assignmentToUpdate.Id)
                .ReturnsNull();

            // Act
            var result = await _sut.UpdateAsync(assignmentToUpdate);

            // Assert
            result.Value.Should().BeNull();
            result.IsError.Should().BeTrue();
            result.FirstError.Should().BeEquivalentTo(AssignmentErrors.NotFound(assignmentToUpdate.Id).FirstError);
        }

        [Theory]
        [ClassData(typeof(UpdateAssignmentDtoFakeProvider))]
        public async Task UpdateAsync_ShouldUpdateEntity_WhenAssignmentHasBeenFound(UpdateAssignmentDto assignmentDto)
        {
            // Arrange
            var assignmentEntity = new AssignmentEntity
            {
                Id = assignmentDto.Id,
                Title = assignmentDto.Title,
                Description = assignmentDto.Description,
                Deadline = assignmentDto.Deadline,
                StartDate = assignmentDto.StartDate.GetValueOrDefault(DateTime.UtcNow),
                MaxGrade = assignmentDto.MaxGrade,
                Status = AssignmentStatus.PassedDeadline,
                Type = assignmentDto.Type,
            };
            _assignmentRepositoryMock
                .GetById(assignmentDto.Id)!
                .Returns(Task.FromResult(assignmentEntity));

            // Act
            var result = await _sut.UpdateAsync(assignmentDto);

            // Assert
            result.Value.Should().NotBeNull();
            result.IsError.Should().BeFalse();
            result.Value.Id.Should().Be(assignmentEntity.Id);
            result.Value.Title.Should().Be(assignmentEntity.Title);
            result.Value.Description.Should().Be(assignmentEntity.Description);
            result.Value.Deadline.Should().Be(assignmentEntity.Deadline);
            result.Value.StartDate.Should().Be(assignmentEntity.StartDate);
            result.Value.MaxGrade.Should().Be(assignmentEntity.MaxGrade);
            result.Value.Status.Should().Be(assignmentEntity.Status);
            result.Value.Type.Should().Be(assignmentEntity.Type);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteEntity()
        {
            // Act
            await _sut.DeleteAsync(Arg.Any<long>());

            // Assert
            _assignmentRepositoryMock.Received(1);
        }
    }
}
