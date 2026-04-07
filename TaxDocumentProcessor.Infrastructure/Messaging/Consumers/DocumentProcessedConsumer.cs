using MassTransit;
using TaxDocumentProcessor.Application.Events;
using TaxDocumentProcessor.Infrastructure.Persistence.MongoDB;
using TaxDocumentProcessor.Infrastructure.Persistence.MongoDB.Entities;

namespace TaxDocumentProcessor.Infrastructure.Messaging.Consumers
{
    public class DocumentProcessedConsumer : IConsumer<DocumentProcessedEvent>
    {
        private readonly ProcessingLogRepository _repository;

        public DocumentProcessedConsumer(ProcessingLogRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<DocumentProcessedEvent> context)
        {
            var evt = context.Message;

            var log = new ProcessingLog
            {
                Id = evt.DocumentId,
                ChaveNota = evt.ChaveNota,
                TipoDocumento = evt.TipoDocumento,
                CnpjEmit = evt.CnpjEmit,
                TotalValue = evt.TotalValue,
                DtEmission = evt.DtEmission,
                ProcessedAt = evt.ProcessedAt
            };

            await _repository.InsertAsync(log, context.CancellationToken);
        }
    }
}
