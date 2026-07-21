using FiscalBridge.Infrastructure.Security;
using Microsoft.Extensions.Options;

namespace FiscalBridge.Api.Tests.Infrastructure;

public sealed class AccessTokenServiceTests
{
    private const string GenerationToken = "generation-token";
    private const string SigningKey = "a-signing-key-with-at-least-32-characters";

    [Fact]
    public void Generate_CreatesValidAccessToken()
    {
        var timeProvider = new MutableTimeProvider(
            new DateTimeOffset(2026, 7, 21, 12, 0, 0, TimeSpan.Zero));
        var service = CreateService(timeProvider);

        var generatedToken = service.Generate();

        Assert.True(service.IsAccessTokenValid(generatedToken.Value));
        Assert.Equal(timeProvider.GetUtcNow().AddMinutes(60), generatedToken.ExpiresAtUtc);
    }

    [Fact]
    public void IsAccessTokenValid_AfterExpiration_ReturnsFalse()
    {
        var timeProvider = new MutableTimeProvider(
            new DateTimeOffset(2026, 7, 21, 12, 0, 0, TimeSpan.Zero));
        var service = CreateService(timeProvider);
        var generatedToken = service.Generate();

        timeProvider.Advance(TimeSpan.FromMinutes(61));

        Assert.False(service.IsAccessTokenValid(generatedToken.Value));
    }

    [Theory]
    [InlineData(GenerationToken, true)]
    [InlineData("invalid", false)]
    [InlineData(null, false)]
    public void IsGenerationTokenValid_ReturnsExpectedResult(string? token, bool expected)
    {
        var service = CreateService(new MutableTimeProvider(DateTimeOffset.UtcNow));

        Assert.Equal(expected, service.IsGenerationTokenValid(token));
    }

    private static AccessTokenService CreateService(TimeProvider timeProvider)
    {
        var options = Options.Create(new TokenOptions
        {
            GenerationToken = GenerationToken,
            AccessTokenSigningKey = SigningKey,
            AccessTokenLifetimeMinutes = 60
        });

        return new AccessTokenService(options, timeProvider);
    }

    private sealed class MutableTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        private DateTimeOffset _utcNow = utcNow;

        public override DateTimeOffset GetUtcNow() => _utcNow;

        public void Advance(TimeSpan duration) => _utcNow += duration;
    }
}
