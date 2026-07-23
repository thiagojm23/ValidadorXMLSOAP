using System.Security.Claims;
using System.Text.Encodings.Web;
using FiscalBridge.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace FiscalBridge.Api.Security;

public sealed class AccessTokenAuthenticationHandler(
    IOptionsMonitor<AccessTokenAuthenticationOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    AccessTokenService accessTokenService)
    : AuthenticationHandler<AccessTokenAuthenticationOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(
                AccessTokenAuthenticationDefaults.HeaderName,
                out var accessToken))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        if (!accessTokenService.IsAccessTokenValid(accessToken.ToString()))
        {
            return Task.FromResult(AuthenticateResult.Fail("Token de acesso inválido ou expirado."));
        }

        var identity = new ClaimsIdentity(
            [new Claim(ClaimTypes.NameIdentifier, "fiscal-bridge-client")],
            Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
