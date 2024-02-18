using System.Diagnostics.CodeAnalysis;

namespace Fusion.Core.Auth;

[ExcludeFromCodeCoverage]
public record RoleDto
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
}