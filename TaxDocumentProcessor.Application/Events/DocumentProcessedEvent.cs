namespace TaxDocumentProcessor.Application.Events
{
    public record DocumentProcessedEvent(
        Guid DocumentId,
        string ChaveNota,
        string TipoDocumento,
        string CnpjEmit,
        string TotalValue,
        DateTime DtEmission,
        DateTime ProcessedAt
    );
}
