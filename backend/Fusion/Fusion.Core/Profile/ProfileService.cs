using ErrorOr;
using Fusion.Infrastructure.Database.Abstractions;
using Fusion.Infrastructure.Profile;

namespace Fusion.Core.Profile;

public interface IProfileService
{
    Task<ErrorOr<ProfileDto>> Get(long profileId);
    Task<ErrorOr<ProfileDto>> Create(ProfileDto profileDto);
    Task<ErrorOr<ProfileDto>> Update(ProfileDto profileDto);
    Task Delete(long profileId);
}

public class ProfileService(
    IProfileRepository profileRepository) : IProfileService
{
    public async Task<ErrorOr<ProfileDto>> Get(long profileId)
    {
        var entity = await profileRepository.GetById(profileId);
        if (entity is null)
        {
            return ProfileErrors.NotFound(profileId);
        }

        var dto = new ProfileDto
        {
            Id = entity.Id,
            Email = entity.Email,
            FirstName = entity.FirstName,
            LastName = entity.LastName
        };

        return dto;
    }

    public async Task<ErrorOr<ProfileDto>> Create(ProfileDto profileDto)
    {
        // we have to ensure that auth0 account is also deleted

        ArgumentNullException.ThrowIfNull(profileDto);

        var spec = ProfileSpecs.ByEmail(profileDto.Email);
        var isDuplicate = await profileRepository.ExistsBySpecification(spec);
        if (isDuplicate)
        {
            return ProfileErrors.Duplicate(profileDto.Email);
        }

        var entity = new ProfileEntity
        {
            FirstName = profileDto.FirstName,
            LastName = profileDto.LastName,
            Email = profileDto.Email,
        };

        entity = await profileRepository.Create(entity);

        profileDto = profileDto with { Id = entity.Id };
        return profileDto;
    }

    public async Task<ErrorOr<ProfileDto>> Update(ProfileDto profileDto)
    {
        // we have to ensure that auth0 account is also updated

        ArgumentNullException.ThrowIfNull(profileDto);

        var existingProfile = await profileRepository.GetById(profileDto.Id);
        if (existingProfile is null)
        {
            return ProfileErrors.NotFound(profileDto.Id);
        }

        // use some mapping tools... or static mapping
        existingProfile.FirstName = profileDto.FirstName;
        existingProfile.LastName = profileDto.LastName;
        existingProfile.Email = profileDto.Email;

        await profileRepository.Update(profileDto.Id, existingProfile);
        return profileDto;
    }

    public async Task Delete(long profileId)
    {
        await profileRepository.Delete(profileId);
    }
}