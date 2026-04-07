namespace TaxDocumentProcessor.Infrastructure.Persistence.MongoDB.Entities
{
    public class ProcessingLog
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public string ChaveNota { get; init; } = string.Empty;
        public string TipoDocumento { get; init; } = string.Empty;
        public string CnpjEmit { get; init; } = string.Empty;
        public string TotalValue { get; init; } = string.Empty;
        public DateTime DtEmission { get; init; }
        public DateTime ProcessedAt { get; init; }
        public DateTime LoggedAt { get; init; } = DateTime.UtcNow;
    }
}
