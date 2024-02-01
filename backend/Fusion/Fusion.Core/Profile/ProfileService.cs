using ErrorOr;
using Fusion.Infrastructure.Database.Abstractions;
using Fusion.Infrastructure.Profile;

namespace Fusion.Core.Profile;

public interface IProfileService
{
    Task<ErrorOr<ProfileDto>> Create(ProfileDto profileDto);
}

public class ProfileService(
    IProfileRepository profileRepository) : IProfileService
{
    public async Task<ErrorOr<ProfileDto>> Create(ProfileDto profileDto)
    {
        ArgumentNullException.ThrowIfNull(profileDto);

        var spec = ProfileSpecs.ByEmail(profileDto.Email);
        var isDuplicate = await profileRepository.ExistsBySpecification(spec);
        if (isDuplicate)
        {
            return ProfileErrors.Duplicate;
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
}