namespace Fusion.RestApi.Auth.Models;

/// <summary>
/// 
/// </summary>
public class UserRolesViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<string> Roles { get; set; } = [];
}
