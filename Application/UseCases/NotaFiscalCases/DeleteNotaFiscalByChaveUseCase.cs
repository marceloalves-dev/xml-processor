using Tax_Document_Processor.Domain.Repositories;
using Tax_Document_Processor.Domain.ValueObjects;

namespace Application.UseCases.NotaFiscalCases
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
