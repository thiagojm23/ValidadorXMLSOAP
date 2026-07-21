using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace FiscalBridge.Infrastructure.Security;

public sealed class AccessTokenService(
    IOptions<TokenOptions> options,
    TimeProvider timeProvider)
{
    private readonly TokenOptions _options = options.Value;

    public bool IsGenerationTokenValid(string? token)
    {
        return FixedTimeEquals(token, _options.GenerationToken);
    }

    public GeneratedAccessToken Generate()
    {
        var expiresAtUtc = timeProvider.GetUtcNow()
            .AddMinutes(_options.AccessTokenLifetimeMinutes);
        var payload = $"v1.{Guid.NewGuid():N}.{expiresAtUtc.ToUnixTimeSeconds()}";
        var signature = Sign(payload);

        return new GeneratedAccessToken($"{payload}.{signature}", expiresAtUtc);
    }

    public bool IsAccessTokenValid(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return false;
        }

        var lastSeparator = token.LastIndexOf('.');
        if (lastSeparator <= 0 || lastSeparator == token.Length - 1)
        {
            return false;
        }

        var payload = token[..lastSeparator];
        var providedSignature = token[(lastSeparator + 1)..];
        var payloadParts = payload.Split('.');

        if (payloadParts.Length != 3 || payloadParts[0] != "v1" ||
            !long.TryParse(payloadParts[2], NumberStyles.None, CultureInfo.InvariantCulture,
                out var expiresAtUnixSeconds))
        {
            return false;
        }

        var expectedSignature = Sign(payload);
        return FixedTimeEquals(providedSignature, expectedSignature) &&
               timeProvider.GetUtcNow().ToUnixTimeSeconds() < expiresAtUnixSeconds;
    }

    private string Sign(string payload)
    {
        var key = Encoding.UTF8.GetBytes(_options.AccessTokenSigningKey);
        var data = Encoding.UTF8.GetBytes(payload);
        return WebEncoders.Base64UrlEncode(HMACSHA256.HashData(key, data));
    }

    private static bool FixedTimeEquals(string? left, string? right)
    {
        if (left is null || right is null)
        {
            return false;
        }

        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(left),
            Encoding.UTF8.GetBytes(right));
    }
}
