using FiscalBridge.Domain.Documents;

namespace FiscalBridge.Application.Validation;

public sealed class NFeValidationStrategy : IFiscalDocumentValidationStrategy
{
    public FiscalDocumentType SupportedType => FiscalDocumentType.NFe;

    public Task<ValidationResult> ValidateAsync(
        FiscalDocument document,
        CancellationToken cancellationToken = default)
    {
        // As regras específicas de NF-e serão implementadas aqui.
        return Task.FromResult(new ValidationResult(
            document.Type,
            nameof(NFeValidationStrategy),
            "Strategy de NF-e selecionada; validação específica ainda não implementada."));
    }
}
