using FiscalBridge.Domain.Documents;

namespace FiscalBridge.Application.Validation;

public sealed class ValidateFiscalDocument(
    IFiscalDocumentValidationStrategyResolver strategyResolver)
{
    public Task<ValidationResult> ExecuteAsync(
        FiscalDocument document,
        CancellationToken cancellationToken = default)
    {
        var strategy = strategyResolver.Resolve(document.Type);
        return strategy.ValidateAsync(document, cancellationToken);
    }
}
