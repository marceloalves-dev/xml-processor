using TaxDocumentProcessor.Application.DTOs;
using System.Linq.Expressions;
using TaxDocumentProcessor.Domain.Entities;
using TaxDocumentProcessor.Domain.Repositories;

namespace TaxDocumentProcessor.Application.UseCases.NotaFiscalCases
{
    public class ListNotaFiscalUseCase
    {
        private readonly INotaFiscalRepository _repository;

        public ListNotaFiscalUseCase(INotaFiscalRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResultDto<NotaFiscalResponseDto>> ExecuteAsync(NotaFiscalFilterDto filtro, CancellationToken cancellationToken = default)
        {
            Expression<Func<NotaFiscal, bool>> filter = x =>
                    (!filtro.DtEmission.HasValue || x.DtEmission == filtro.DtEmission) &&
                    (string.IsNullOrEmpty(filtro.RazaoSocial) || x.RazaoSocial == filtro.RazaoSocial) &&
                    (filtro.CnpjEmit == null || x.CnpjEmit == filtro.CnpjEmit);

            var itemsTask = _repository.ListAsync(filter, filtro.Page, filtro.PageSize, cancellationToken);
            var totalTask = _repository.CountAsync(filter, cancellationToken);
            await Task.WhenAll(itemsTask, totalTask);

            return new PagedResultDto<NotaFiscalResponseDto>
            {
                Items = itemsTask.Result.Select(NotaFiscalResponseDto.From).ToList(),
                Page = filtro.Page,
                PageSize = filtro.PageSize,
                Total = totalTask.Result
            };
        }
    }
}
