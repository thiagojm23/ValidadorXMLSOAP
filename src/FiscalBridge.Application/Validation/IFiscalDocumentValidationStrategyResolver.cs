using FiscalBridge.Domain.Documents;

namespace FiscalBridge.Application.Validation;

public interface IFiscalDocumentValidationStrategyResolver
{
    IFiscalDocumentValidationStrategy Resolve(FiscalDocumentType type);
}
