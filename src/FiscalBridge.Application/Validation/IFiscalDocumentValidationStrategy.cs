using FiscalBridge.Domain.Documents;

namespace FiscalBridge.Application.Validation;

public interface IFiscalDocumentValidationStrategy
{
    FiscalDocumentType SupportedType { get; }

    Task<ValidationResult> ValidateAsync(
        FiscalDocument document,
        CancellationToken cancellationToken = default);
}
