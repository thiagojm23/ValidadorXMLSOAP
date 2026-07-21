using FiscalBridge.Api.Contracts;
using FiscalBridge.Application.Validation;
using FiscalBridge.Domain;
using FiscalBridge.Domain.Documents;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FiscalBridge.Api.Endpoints;

public static class FiscalDocumentEndpoints
{
    public static IEndpointRouteBuilder MapFiscalDocumentEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/notas")
            .WithTags("Notas fiscais");

        group.MapPost("/validar-xml", ValidateXmlAsync)
            .WithName("ValidarXML")
            .WithSummary("Seleciona a strategy correspondente ao tipo da nota fiscal.");

        return endpoints;
    }

    private static async Task<Results<Ok<ValidateXmlResponse>, BadRequest<ProblemDetails>>> ValidateXmlAsync(
        ValidateXmlRequest request,
        ValidateFiscalDocument fiscalDocumentValidator,
        CancellationToken cancellationToken)
    {
        if (!TryParseDocumentType(request.TipoNota, out var documentType))
        {
            return BadRequest("TipoNota deve ser 'CTe' ou 'NFe'.");
        }

        try
        {
            var document = FiscalDocument.Create(request.Xml ?? string.Empty, documentType);
            var result = await fiscalDocumentValidator.ExecuteAsync(document, cancellationToken);

            return TypedResults.Ok(new ValidateXmlResponse(
                result.Type.ToString(),
                result.Strategy,
                result.Message));
        }
        catch (DomainException exception)
        {
            return BadRequest(exception.Message);
        }
    }

    private static bool TryParseDocumentType(
        string? value,
        out FiscalDocumentType documentType)
    {
        if (string.Equals(value, "CTe", StringComparison.OrdinalIgnoreCase))
        {
            documentType = FiscalDocumentType.CTe;
            return true;
        }

        if (string.Equals(value, "NFe", StringComparison.OrdinalIgnoreCase))
        {
            documentType = FiscalDocumentType.NFe;
            return true;
        }

        documentType = default;
        return false;
    }

    private static BadRequest<ProblemDetails> BadRequest(string detail)
    {
        return TypedResults.BadRequest(new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Requisição inválida",
            Detail = detail
        });
    }
}
