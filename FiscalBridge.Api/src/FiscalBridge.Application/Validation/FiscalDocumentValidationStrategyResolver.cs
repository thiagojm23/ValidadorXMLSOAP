using FiscalBridge.Domain.Documents;

namespace FiscalBridge.Application.Validation;

public sealed class FiscalDocumentValidationStrategyResolver : IFiscalDocumentValidationStrategyResolver
{
    private readonly IReadOnlyDictionary<FiscalDocumentType, IFiscalDocumentValidationStrategy> _strategies;

    public FiscalDocumentValidationStrategyResolver(
        IEnumerable<IFiscalDocumentValidationStrategy> strategies)
    {
        _strategies = strategies.ToDictionary(strategy => strategy.SupportedType);
    }

    public IFiscalDocumentValidationStrategy Resolve(FiscalDocumentType type)
    {
        if (_strategies.TryGetValue(type, out var strategy))
        {
            return strategy;
        }

        throw new NotSupportedException($"Não existe uma strategy para o tipo de nota '{type}'.");
    }
}
