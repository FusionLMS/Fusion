using System.Diagnostics.CodeAnalysis;

namespace Fusion.RestApi.Auth.Models;

/// <summary>
/// Represents the data model in post login flow.
/// </summary>
[ExcludeFromCodeCoverage]
public record PostLoginHandleModel() : PostRegisterHandleModel;