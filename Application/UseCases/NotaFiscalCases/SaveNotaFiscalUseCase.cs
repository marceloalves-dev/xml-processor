using Tax_Document_Processor.Application.Services;
using Tax_Document_Processor.Domain.Repositories;

namespace Application.UseCases.NotaFiscalCases
{
    public class SaveNotaFiscalUseCase
    {
        private readonly INotaFiscalRepository _repository;
        private readonly INotaFiscalParser _notaFiscalParser;


        public SaveNotaFiscalUseCase(INotaFiscalRepository repository, INotaFiscalParser notaFiscalParser)
        {
            _repository = repository;
            _notaFiscalParser = notaFiscalParser;
        }

        public async Task ExecuteAsync(string xml)
        {
            var notaFiscal = _notaFiscalParser.Parse(xml);

            if (await _repository.GetByKeyAsync(notaFiscal.ChaveNota) != null)
                return;

            await _repository.SaveAsync(notaFiscal);
        }

    }
}
