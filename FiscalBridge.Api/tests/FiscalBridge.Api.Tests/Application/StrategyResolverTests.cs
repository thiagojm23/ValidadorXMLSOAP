using FiscalBridge.Application.Validation;
using FiscalBridge.Domain.Documents;

namespace FiscalBridge.Api.Tests.Application;

public sealed class StrategyResolverTests
{
    [Theory]
    [InlineData(FiscalDocumentType.CTe, typeof(CTeValidationStrategy))]
    [InlineData(FiscalDocumentType.NFe, typeof(NFeValidationStrategy))]
    public void Resolve_ReturnsStrategyRegisteredForDocumentType(
        FiscalDocumentType type,
        Type expectedStrategyType)
    {
        IFiscalDocumentValidationStrategy[] strategies =
        [
            new CTeValidationStrategy(),
            new NFeValidationStrategy()
        ];
        var resolver = new FiscalDocumentValidationStrategyResolver(strategies);

        var strategy = resolver.Resolve(type);

        Assert.IsType(expectedStrategyType, strategy);
    }
}
