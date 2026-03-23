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

        public async Task ExecuteAsync(ChaveNota chaveNota)
        {
            var nota = await _repository.GetByKeyAsync(chaveNota);

            if (nota is null)
                throw new Exception("Nota fiscal não encontrada");

            await _repository.DeleteAsync(chaveNota);
        }
    }
}
