using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using FluentAssertions;
using Fusion.Core.Auth;
using Fusion.Infrastructure.Database.Abstractions;
using Fusion.Infrastructure.Profile;
using Fusion.Tests.Core.Auth.Helpers;
using Fusion.Tests.Core.Profile.Helpers;
using NSubstitute;

namespace Fusion.Tests.Core.Auth;

public class AuthServiceTests
{
    private readonly IAuthService _sut;

    private readonly IManagementApiClient _managementApiClient;
    private readonly IProfileRepository _profileRepository;

    public AuthServiceTests()
    {
        _managementApiClient = Substitute.For<IManagementApiClient>();
        _profileRepository = Substitute.For<IProfileRepository>();

        _sut = new AuthService(_managementApiClient, _profileRepository);
    }

    [Fact]
    public async Task AssignUserToRole_RolesNull_ArgumentNullException()
    {
        var faker = new UserRoleDtoFakeProvider();
        var dto = faker.Get();
        dto.Roles = null;

        var action = () => _sut.AssignUserToRole(dto, CancellationToken.None);
        await action.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task AssignUserToRole_Auth0IdNull_ArgumentNullException()
    {
        var profileFaker = new ProfileDtoFakeProvider();
        var profileDto = profileFaker.Get();

        var authRoleFaker = new UserRoleDtoFakeProvider();
        var dto = authRoleFaker.Get();

        var profileEntity = new ProfileEntity
        {
            Id = profileDto.Id,
            FirstName = profileDto.FirstName,
            LastName = profileDto.LastName,
            Email = profileDto.Email,
            Auth0UserId = null
        };

        _profileRepository.GetById(Arg.Any<long>())!
            .Returns(Task.FromResult(profileEntity));

        var result = await _sut.AssignUserToRole(dto, CancellationToken.None);

        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(AuthErrors.ProfileMisconfigured.FirstError);
    }

    [Theory]
    [ClassData(typeof(UserRoleDtoFakeProvider))]
    public async Task AssignUserToRole_RolesValid_Success(UserRoleDto dto)
    {
        var profileFaker = new ProfileDtoFakeProvider();
        var profileDto = profileFaker.Get();

        var profileEntity = new ProfileEntity
        {
            Id = profileDto.Id,
            FirstName = profileDto.FirstName,
            LastName = profileDto.LastName,
            Email = profileDto.Email,
            Auth0UserId = profileDto.Auth0UserId
        };

        _profileRepository.GetById(Arg.Any<long>())!
            .Returns(Task.FromResult(profileEntity));

        var existingRoles = AuthDefaults
            .Roles.All.Values
            .ExceptBy(dto.Roles!, x => x.Name)
            .ToList();
        var pagedList = new PagedList<Role>(existingRoles.Select(x => new Role
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description
        }));
        _managementApiClient.Users.GetRolesAsync(Arg.Any<string>(), cancellationToken: Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(pagedList as IPagedList<Role>));

        _managementApiClient.Users.AssignRolesAsync(
                Arg.Any<string>(), Arg.Any<AssignRolesRequest>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        var result = await _sut.AssignUserToRole(dto, CancellationToken.None);

        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Value.FusionUserId.Should().Be(dto.FusionUserId);
        result.Value.Roles.Should().BeEquivalentTo(AuthDefaults.Roles.All.Keys);
    }

    [Theory]
    [ClassData(typeof(UserRoleDtoFakeProvider))]
    public async Task AssignUserToRole_RolesEquals_Success(UserRoleDto dto)
    {
        var profileFaker = new ProfileDtoFakeProvider();
        var profileDto = profileFaker.Get();
        dto.Roles = [];

        var profileEntity = new ProfileEntity
        {
            Id = profileDto.Id,
            FirstName = profileDto.FirstName,
            LastName = profileDto.LastName,
            Email = profileDto.Email,
            Auth0UserId = profileDto.Auth0UserId
        };

        _profileRepository.GetById(Arg.Any<long>())!
            .Returns(Task.FromResult(profileEntity));

        var existingRoles = AuthDefaults
            .Roles.All.Values
            .ExceptBy(dto.Roles!, x => x.Name)
            .ToList();
        var pagedList = new PagedList<Role>(existingRoles.Select(x => new Role
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description
        }));
        _managementApiClient.Users.GetRolesAsync(Arg.Any<string>(), cancellationToken: Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(pagedList as IPagedList<Role>));

        _managementApiClient.Users.AssignRolesAsync(
                Arg.Any<string>(), Arg.Any<AssignRolesRequest>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        var result = await _sut.AssignUserToRole(dto, CancellationToken.None);

        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Value.FusionUserId.Should().Be(dto.FusionUserId);
        result.Value.Roles.Should().BeEquivalentTo(dto.Roles);
    }

    [Fact]
    public async Task UnAssignUserToRole_RolesNull_ArgumentNullException()
    {
        var faker = new UserRoleDtoFakeProvider();
        var dto = faker.Get();
        dto.Roles = null;

        var action = () => _sut.UnAssignUserFromRole(dto, CancellationToken.None);
        await action.Should().ThrowExactlyAsync<ArgumentNullException>();
    }

    [Theory]
    [ClassData(typeof(UserRoleDtoFakeProvider))]
    public async Task UnAssignUserToRole_RolesValid_Success(UserRoleDto dto)
    {
        var profileFaker = new ProfileDtoFakeProvider();
        var profileDto = profileFaker.Get();

        var profileEntity = new ProfileEntity
        {
            Id = profileDto.Id,
            FirstName = profileDto.FirstName,
            LastName = profileDto.LastName,
            Email = profileDto.Email,
            Auth0UserId = profileDto.Auth0UserId
        };

        _profileRepository.GetById(Arg.Any<long>())!
            .Returns(Task.FromResult(profileEntity));

        var existingRoles = AuthDefaults.Roles.All.Values;
        var pagedList = new PagedList<Role>(existingRoles.Select(x => new Role
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description
        }));
        _managementApiClient.Users.GetRolesAsync(
                Arg.Any<string>(), cancellationToken: Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(pagedList as IPagedList<Role>));

        _managementApiClient.Users.RemoveRolesAsync(
                Arg.Any<string>(), Arg.Any<AssignRolesRequest>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        var result = await _sut.UnAssignUserFromRole(dto, CancellationToken.None);

        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Value.FusionUserId.Should().Be(dto.FusionUserId);

        var expectedRoleIds = AuthDefaults.Roles.All
            .Where(x => !dto.Roles!.Contains(x.Key))
            .Select(x => x.Value.Id);
        result.Value.Roles.Should().BeEquivalentTo(expectedRoleIds);
    }

    
    [Theory]
    [ClassData(typeof(UserRoleDtoFakeProvider))]
    public async Task UnAssignUserToRole_RolesEmpty_Success(UserRoleDto dto)
    {
        var profileFaker = new ProfileDtoFakeProvider();
        var profileDto = profileFaker.Get();
        dto.Roles = [];

        var profileEntity = new ProfileEntity
        {
            Id = profileDto.Id,
            FirstName = profileDto.FirstName,
            LastName = profileDto.LastName,
            Email = profileDto.Email,
            Auth0UserId = profileDto.Auth0UserId
        };

        _profileRepository.GetById(Arg.Any<long>())!
            .Returns(Task.FromResult(profileEntity));

        var existingRoles = AuthDefaults.Roles.All.Values;
        var pagedList = new PagedList<Role>(existingRoles.Select(x => new Role
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description
        }));
        _managementApiClient.Users.GetRolesAsync(
                Arg.Any<string>(), cancellationToken: Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(pagedList as IPagedList<Role>));

        _managementApiClient.Users.RemoveRolesAsync(
                Arg.Any<string>(), Arg.Any<AssignRolesRequest>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        var result = await _sut.UnAssignUserFromRole(dto, CancellationToken.None);

        result.Should().NotBeNull();
        result.IsError.Should().BeFalse();
        result.Value.FusionUserId.Should().Be(dto.FusionUserId);
        result.Value.Roles.Should().BeEquivalentTo([]);
    }

    [Fact]
    public async Task UnAssignUserToRole_Auth0IdNull_ArgumentNullException()
    {
        var profileFaker = new ProfileDtoFakeProvider();
        var profileDto = profileFaker.Get();

        var authRoleFaker = new UserRoleDtoFakeProvider();
        var dto = authRoleFaker.Get();

        var profileEntity = new ProfileEntity
        {
            Id = profileDto.Id,
            FirstName = profileDto.FirstName,
            LastName = profileDto.LastName,
            Email = profileDto.Email,
            Auth0UserId = null
        };

        _profileRepository.GetById(Arg.Any<long>())!
            .Returns(Task.FromResult(profileEntity));

        var result = await _sut.UnAssignUserFromRole(dto, CancellationToken.None);

        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(AuthErrors.ProfileMisconfigured.FirstError);
    }
}