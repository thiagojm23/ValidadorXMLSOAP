using FiscalBridge.Api.Contracts;
using FiscalBridge.Infrastructure.Security;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FiscalBridge.Api.Endpoints;

public static class TokenEndpoints
{
    private const string GenerationTokenHeader = "tokenGeracao";

    public static IEndpointRouteBuilder MapTokenEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/tokens", GenerateToken)
            .AllowAnonymous()
            .WithName("GerarTokenAcesso")
            .WithSummary("Gera um token de acesso usando o token privado de geração.");

        return endpoints;
    }

    private static Results<Ok<GenerateTokenResponse>, UnauthorizedHttpResult> GenerateToken(
        HttpContext httpContext,
        AccessTokenService accessTokenService)
    {
        var generationToken = httpContext.Request.Headers[GenerationTokenHeader].ToString();

        if (!accessTokenService.IsGenerationTokenValid(generationToken))
        {
            return TypedResults.Unauthorized();
        }

        var generatedToken = accessTokenService.Generate();
        return TypedResults.Ok(new GenerateTokenResponse(
            generatedToken.Value,
            generatedToken.ExpiresAtUtc));
    }
}
