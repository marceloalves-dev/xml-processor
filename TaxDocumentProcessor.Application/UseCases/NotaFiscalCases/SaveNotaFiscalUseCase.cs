using TaxDocumentProcessor.Application.Events;
using TaxDocumentProcessor.Application.Interfaces;
using TaxDocumentProcessor.Application.Services;
using TaxDocumentProcessor.Domain.Repositories;

namespace TaxDocumentProcessor.Application.UseCases.NotaFiscalCases
{
    public class SaveNotaFiscalUseCase
    {
        private readonly INotaFiscalRepository _repository;
        private readonly INotaFiscalParser _notaFiscalParser;
        private readonly IEventPublisher _eventPublisher;

        public SaveNotaFiscalUseCase(INotaFiscalRepository repository, INotaFiscalParser notaFiscalParser, IEventPublisher eventPublisher)
        {
            _repository = repository;
            _notaFiscalParser = notaFiscalParser;
            _eventPublisher = eventPublisher;
        }

        /// <returns>true if saved, false if already existed</returns>
        public async Task<bool> ExecuteAsync(string xml, CancellationToken cancellationToken = default)
        {
            var notaFiscal = _notaFiscalParser.Parse(xml);
            var saved = await _repository.SaveAsync(notaFiscal, cancellationToken);

            if (saved)
            {
                var evento = new DocumentProcessedEvent(
                    DocumentId: notaFiscal.Id,
                    ChaveNota: notaFiscal.ChaveNota.Value,
                    TipoDocumento: notaFiscal.GetType().Name,
                    CnpjEmit: notaFiscal.CnpjEmit.Value,
                    TotalValue: notaFiscal.TotalValue,
                    DtEmission: notaFiscal.DtEmission,
                    ProcessedAt: DateTime.UtcNow
                );
                await _eventPublisher.PublishAsync(evento, cancellationToken);
            }

            return saved;
        }
    }
}
