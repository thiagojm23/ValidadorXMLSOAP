using FiscalBridge.Domain.Documents;

namespace FiscalBridge.Application.Validation;

public sealed class CTeValidationStrategy : IFiscalDocumentValidationStrategy
{
    public FiscalDocumentType SupportedType => FiscalDocumentType.CTe;

    public Task<ValidationResult> ValidateAsync(
        FiscalDocument document,
        CancellationToken cancellationToken = default)
    {
        // As regras específicas de CT-e serão implementadas aqui.
        return Task.FromResult(new ValidationResult(
            document.Type,
            nameof(CTeValidationStrategy),
            "Strategy de CT-e selecionada; validação específica ainda não implementada."));
    }
}
