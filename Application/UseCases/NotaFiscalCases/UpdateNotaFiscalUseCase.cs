using Application.DTOs;
using Tax_Document_Processor.Domain.Entities;
using Tax_Document_Processor.Domain.Repositories;
using Tax_Document_Processor.Domain.ValueObjects;

namespace Application.UseCases.NotaFiscalCases
{
    public class UpdateNotaFiscalUseCase
    {
        private readonly INotaFiscalRepository _repository;

        public UpdateNotaFiscalUseCase(INotaFiscalRepository repository)
        {
            _repository = repository;
        }

        public async Task<NotaFiscal?> ExecuteAsync(ChaveNota chave, UpdateNotaFiscalRequest request, CancellationToken cancellationToken = default)
        {
            var nota = await _repository.GetByKeyAsync(chave, cancellationToken);

            if (nota is null)
                return null;

            nota.Update(request.RazaoSocial, request.TotalValue);
            await _repository.UpdateAsync(chave, nota, cancellationToken);
            return nota;
        }
    }
}
