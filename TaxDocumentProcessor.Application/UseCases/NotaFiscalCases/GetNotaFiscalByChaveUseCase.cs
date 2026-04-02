using TaxDocumentProcessor.Application.DTOs;
using TaxDocumentProcessor.Domain.Repositories;
using TaxDocumentProcessor.Domain.ValueObjects;

namespace TaxDocumentProcessor.Application.UseCases.NotaFiscalCases
{
    public class GetNotaFiscalByChaveUseCase
    {
        private readonly INotaFiscalRepository _repository;

        public GetNotaFiscalByChaveUseCase(INotaFiscalRepository repository)
        {
            _repository = repository;
        }

        public async Task<NotaFiscalResponseDto?> ExecuteAsync(ChaveNota chaveNota, CancellationToken cancellationToken = default)
        {
            var nota = await _repository.GetByKeyAsync(chaveNota, cancellationToken);
            return nota is null ? null : NotaFiscalResponseDto.From(nota);
        }
    }
}
