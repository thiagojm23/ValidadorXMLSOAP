namespace FiscalBridge.Domain.Documents;

public sealed class FiscalDocument
{
    private FiscalDocument(string xml, FiscalDocumentType type)
    {
        Xml = xml;
        Type = type;
    }

    public string Xml { get; }
    public FiscalDocumentType Type { get; }

    public static FiscalDocument Create(string xml, FiscalDocumentType type)
    {
        if (string.IsNullOrWhiteSpace(xml))
        {
            throw new DomainException("O XML da nota é obrigatório.");
        }

        if (!Enum.IsDefined(type))
        {
            throw new DomainException("O tipo da nota é inválido.");
        }

        return new FiscalDocument(xml, type);
    }
}
