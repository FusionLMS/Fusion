namespace Fusion.Core.Auth;

public class UserRoleDto
{
    public long FusionUserId { get; set; }
    public List<string>? Roles { get; set; }
}
