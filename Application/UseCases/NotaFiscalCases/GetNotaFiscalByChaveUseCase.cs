using Tax_Document_Processor.Domain.Entities;
using Tax_Document_Processor.Domain.Repositories;
using Tax_Document_Processor.Domain.ValueObjects;

namespace Application.UseCases.NotaFiscalCases
{
    public class GetNotaFiscalByChaveUseCase
    {
        private readonly INotaFiscalRepository _repository;

        public GetNotaFiscalByChaveUseCase(INotaFiscalRepository repository)
        {
            _repository = repository;
        }

        public async Task<NotaFiscal?> ExecuteAsync(ChaveNota chaveNota, CancellationToken cancellationToken = default)
        {
            return await _repository.GetByKeyAsync(chaveNota, cancellationToken);
        }
    }
}
