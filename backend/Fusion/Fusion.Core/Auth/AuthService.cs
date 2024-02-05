using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using ErrorOr;
using Fusion.Infrastructure.Database.Abstractions;
using Fusion.Infrastructure.Profile;

namespace Fusion.Core.Auth;

public interface IAuthService
{
    Task<ErrorOr<UserRoleDto>> AssignUserToRole(UserRoleDto roleDto, CancellationToken ct);
    Task<ErrorOr<UserRoleDto>> UnAssignUserFromRole(UserRoleDto roleDto, CancellationToken ct);
}

public class AuthService(
    IManagementApiClient managementApiClient,
    IProfileRepository profileRepository) : IAuthService
{
    public async Task<ErrorOr<UserRoleDto>> AssignUserToRole(
        UserRoleDto roleDto, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(roleDto.Roles);

        var profile = await profileRepository.GetById(roleDto.FusionUserId);
        if (profile?.Auth0UserId is null)
        {
            return AuthErrors.ProfileMisconfigured;
        }

        return await AssignRolesInternal(profile.Auth0UserId, roleDto, ct);
    }

    private async Task<ErrorOr<UserRoleDto>> AssignRolesInternal(
        string auth0UserId,
        UserRoleDto roleDto,
        CancellationToken ct)
    {
        var assignedRoles = await managementApiClient.Users
            .GetRolesAsync(auth0UserId, cancellationToken: ct);

        var newRoles = roleDto.Roles!
            .Except(assignedRoles.Select(x => x.Name));

        var newRolesIds = AuthDefaults.Roles.All
            .Where(x => newRoles.Contains(x.Key))
            .Select(x => x.Value.Id)
            .ToArray();

        if (newRolesIds.Length is 0)
        {
            return roleDto;
        }        

        await managementApiClient.Users.AssignRolesAsync(
            auth0UserId, new AssignRolesRequest { Roles = newRolesIds }, ct);

        var updatedRoles = assignedRoles.Select(x => x.Name).Union(newRoles);

        return new UserRoleDto
        {
            FusionUserId = roleDto.FusionUserId,
            Roles = updatedRoles.ToList()
        };
    }

    public async Task<ErrorOr<UserRoleDto>> UnAssignUserFromRole(
        UserRoleDto roleDto, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(roleDto.Roles);

        var profile = await profileRepository.GetById(roleDto.FusionUserId);
        if (profile?.Auth0UserId is null)
        {
            return AuthErrors.ProfileMisconfigured;
        }

        return await UnAssignRolesInternal(roleDto, ct, profile);
    }

    private async Task<ErrorOr<UserRoleDto>> UnAssignRolesInternal(
        UserRoleDto roleDto, CancellationToken ct, ProfileEntity profile)
    {
        var assignedRoles = await managementApiClient.Users
            .GetRolesAsync(profile.Auth0UserId, cancellationToken: ct);

        var roleIds = assignedRoles
            .Where(x => roleDto.Roles!.Contains(x.Name))
            .Select(r => r.Id)
            .ToArray();

        if (roleIds.Length is 0)
        {
            return roleDto;
        }

        await managementApiClient.Users.RemoveRolesAsync(
            profile.Auth0UserId, new AssignRolesRequest { Roles = roleIds }, ct);

        var updatedRoles = assignedRoles
            .Where(x => !roleDto.Roles!.Contains(x.Name))
            .Select(r => r.Id);

        return new UserRoleDto
        {
            FusionUserId = roleDto.FusionUserId,
            Roles = updatedRoles.ToList()
        };
    }
}