namespace FiscalBridge.Infrastructure.Security;

public sealed class TokenOptions
{
    public const string SectionName = "Security";

    public string GenerationToken { get; init; } = string.Empty;
    public string AccessTokenSigningKey { get; init; } = string.Empty;
    public int AccessTokenLifetimeMinutes { get; init; } = 60;
}
