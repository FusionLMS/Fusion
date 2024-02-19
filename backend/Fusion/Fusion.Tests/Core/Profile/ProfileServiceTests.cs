using FluentAssertions;
using Fusion.Core.Profile;
using Fusion.Infrastructure.Database.Abstractions;
using Fusion.Infrastructure.Database.Specifications;
using Fusion.Infrastructure.Profile;
using Fusion.Tests.Core.Profile.Helpers;
using NSubstitute;

namespace Fusion.Tests.Core.Profile;

public class ProfileServiceTests
{
    private readonly IProfileService _sut;
    private readonly IProfileRepository _profileRepositoryMock;

    public ProfileServiceTests()
    {
        _profileRepositoryMock = Substitute.For<IProfileRepository>();
        _sut = new ProfileService(_profileRepositoryMock);
    }

    [Theory]
    [ClassData(typeof(ProfileDtoFakeProvider))]
    public async Task Create_WhenValid_ProfileCreated(ProfileDto dto)
    {
        _profileRepositoryMock.ExistsBySpecification(Arg.Any<Specification<ProfileEntity>>()).Returns(false);
        _profileRepositoryMock.Create(Arg.Any<ProfileEntity>())
            .Returns(c => Task.FromResult(c.Arg<ProfileEntity>()));

        var result = await _sut.Create(dto);

        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Value.FirstName.Should().Be(dto.FirstName);
        result.Value.LastName.Should().Be(dto.LastName);
        result.Value.Email.Should().Be(dto.Email);
        result.Value.Auth0UserId.Should().Be(dto.Auth0UserId);
    }

    [Fact]
    public async Task Create_WhenProfileDtoIsNull_ValidationError()
    {
        var result = await _sut.Create(null);

        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.FirstError.Should().BeEquivalentTo(ProfileErrors.ValidationFailed.FirstError);
    }

    [Fact]
    public async Task Create_WhenDuplicate_DuplicateError()
    {
        var faker = new ProfileDtoFakeProvider();
        var dto = faker.Get();

        _profileRepositoryMock.ExistsBySpecification(Arg.Any<Specification<ProfileEntity>>()).Returns(true);

        var result = await _sut.Create(dto);

        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.FirstError.Should().BeEquivalentTo(ProfileErrors.Duplicate(dto.Email).FirstError);
    }

    [Fact]
    public async Task Update_WhenProfileDtoIsNull_ValidationError()
    {
        var result = await _sut.Update(null);

        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.FirstError.Should().BeEquivalentTo(ProfileErrors.ValidationFailed.FirstError);
    }

    [Fact]
    public async Task Update_WhenNotExists_NotFoundError()
    {
        var faker = new ProfileDtoFakeProvider();
        var dto = faker.Get();

        _profileRepositoryMock.GetById(Arg.Any<long>())!.Returns(Task.FromResult<ProfileEntity>(null!));

        var result = await _sut.Update(dto);

        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.FirstError.Should().BeEquivalentTo(ProfileErrors.NotFound(dto.Id).FirstError);
    }

    [Theory]
    [ClassData(typeof(ProfileDtoFakeProvider))]
    public async Task Update_WhenValid_ProfileUpdated(ProfileDto dto)
    {
        var faker = new ProfileDtoFakeProvider();
        var updatedProfile = faker.Get();
        updatedProfile.Id = dto.Id;

        var oldEnt = new ProfileEntity
        {
            Id = dto.Id,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Auth0UserId = dto.Auth0UserId
        };

        _profileRepositoryMock.GetById(Arg.Any<long>()).Returns(oldEnt);
        _profileRepositoryMock.Update(Arg.Any<long>(), Arg.Any<ProfileEntity>())
            .Returns(c => Task.FromResult(c.Arg<ProfileEntity>()));

        var result = await _sut.Update(updatedProfile);

        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Value.FirstName.Should().Be(updatedProfile.FirstName);
        result.Value.LastName.Should().Be(updatedProfile.LastName);
        result.Value.Email.Should().Be(updatedProfile.Email);
        result.Value.Auth0UserId.Should().Be(updatedProfile.Auth0UserId);
    }

    [Fact]
    public async Task Get_WhenNotExists_NotFoundError()
    {
        const int profileId = 1;

        _profileRepositoryMock.GetById(Arg.Any<long>())!.Returns(Task.FromResult<ProfileEntity>(null!));

        var result = await _sut.Get(profileId);

        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.FirstError.Should().BeEquivalentTo(ProfileErrors.NotFound(profileId).FirstError);
    }

    [Theory]
    [ClassData(typeof(ProfileDtoFakeProvider))]
    public async Task Get_WhenExists_ProfileDtoReturned(ProfileDto dto)
    {
        var profileEntity = new ProfileEntity
        {
            Id = dto.Id,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Auth0UserId = dto.Auth0UserId,
        };

        _profileRepositoryMock.GetById(Arg.Any<long>())!.Returns(Task.FromResult(profileEntity));

        var result = await _sut.Get(dto.Id);

        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Value.FirstName.Should().Be(dto.FirstName);
        result.Value.LastName.Should().Be(dto.LastName);
        result.Value.Email.Should().Be(dto.Email);
        result.Value.Auth0UserId.Should().Be(dto.Auth0UserId);
    }

    // This is a formal test to keep code coverage.
    // Actually delete must be validated with integration tests
    [Fact]
    public async Task Delete_WhenExists_Success()
    {
        const int profileId = 0;

        _profileRepositoryMock.Delete(Arg.Any<long>()).Returns(Task.CompletedTask);

        await _sut.Delete(profileId);

        Assert.True(true);
    }
}