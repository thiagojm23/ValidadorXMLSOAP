using FiscalBridge.Domain.Documents;

namespace FiscalBridge.Application.Validation;

public sealed record ValidationResult(
    FiscalDocumentType Type,
    string Strategy,
    string Message);
