namespace FiscalBridge.Api.Contracts;

public sealed record GenerateTokenResponse(
    string TokenAcesso,
    DateTimeOffset ExpiraEmUtc);
