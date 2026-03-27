using Application.DTOs;
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

        public async Task<NotaFiscalResponseDto?> ExecuteAsync(ChaveNota chaveNota, CancellationToken cancellationToken = default)
        {
            var nota = await _repository.GetByKeyAsync(chaveNota, cancellationToken);
            return nota is null ? null : NotaFiscalResponseDto.From(nota);
        }
    }
}
