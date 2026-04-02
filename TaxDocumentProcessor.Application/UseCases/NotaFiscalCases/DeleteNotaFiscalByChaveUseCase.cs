using TaxDocumentProcessor.Domain.Repositories;
using TaxDocumentProcessor.Domain.ValueObjects;

namespace TaxDocumentProcessor.Application.UseCases.NotaFiscalCases
{
    public class DeleteNotaFiscalByChaveUseCase
    {
        private readonly INotaFiscalRepository _repository;

        public DeleteNotaFiscalByChaveUseCase(INotaFiscalRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(ChaveNota chaveNota, CancellationToken cancellationToken = default)
        {
            var deleted = await _repository.DeleteAsync(chaveNota, cancellationToken);

            if (!deleted)
                throw new Exception("Nota fiscal não encontrada");
        }
    }
}
