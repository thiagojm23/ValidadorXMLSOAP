namespace FiscalBridge.Api.Contracts;

public sealed record ValidateXmlResponse(
    string TipoNota,
    string Strategy,
    string Mensagem);
