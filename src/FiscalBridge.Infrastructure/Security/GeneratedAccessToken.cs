namespace FiscalBridge.Infrastructure.Security;

public sealed record GeneratedAccessToken(string Value, DateTimeOffset ExpiresAtUtc);
