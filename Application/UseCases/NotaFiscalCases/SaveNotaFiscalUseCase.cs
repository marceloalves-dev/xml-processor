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

        /// <returns>true if saved, false if already existed</returns>
        public async Task<bool> ExecuteAsync(string xml, CancellationToken cancellationToken = default)
        {
            var notaFiscal = _notaFiscalParser.Parse(xml);
            return await _repository.SaveAsync(notaFiscal, cancellationToken);
        }

    }
}
