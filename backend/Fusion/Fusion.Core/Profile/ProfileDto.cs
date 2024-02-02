namespace Fusion.Core.Profile;

public record ProfileDto
{
    public long Id { get; set; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public string? Auth0UserId { get; set; }
}