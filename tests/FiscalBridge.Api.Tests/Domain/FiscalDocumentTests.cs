using FiscalBridge.Domain;
using FiscalBridge.Domain.Documents;

namespace FiscalBridge.Api.Tests.Domain;

public sealed class FiscalDocumentTests
{
    [Fact]
    public void Create_WithValidData_CreatesDocument()
    {
        var document = FiscalDocument.Create("<cte />", FiscalDocumentType.CTe);

        Assert.Equal("<cte />", document.Xml);
        Assert.Equal(FiscalDocumentType.CTe, document.Type);
    }

    [Fact]
    public void Create_WithoutXml_ThrowsDomainException()
    {
        var exception = Assert.Throws<DomainException>(() =>
            FiscalDocument.Create(" ", FiscalDocumentType.NFe));

        Assert.Equal("O XML da nota é obrigatório.", exception.Message);
    }
}
