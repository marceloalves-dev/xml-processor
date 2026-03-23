using Application.DTOs;
using System.Linq.Expressions;
using Tax_Document_Processor.Domain.Entities;
using Tax_Document_Processor.Domain.Repositories;

namespace Application.UseCases.NotaFiscalCases
{
    public class ListNotaFiscalUseCase
    {
        private readonly INotaFiscalRepository _repository;

        public ListNotaFiscalUseCase(INotaFiscalRepository repository)
        {
            _repository = repository;
        }


        public async Task<PagedResultDto<NotaFiscal>> ExecuteAsync(NotaFiscalFilterDto filtro)
        {
            Expression<Func<NotaFiscal, bool>> filter = x =>
                    (!filtro.DtEmission.HasValue || x.DtEmission == filtro.DtEmission) &&
                    (string.IsNullOrEmpty(filtro.RazaoSocial) || x.RazaoSocial == filtro.RazaoSocial) &&
                    (filtro.CnpjEmit == null || x.CnpjEmit == filtro.CnpjEmit);

            var items = await _repository.ListAsync(filter, filtro.Page, filtro.PageSize);
            var total = await _repository.CountAsync(filter);

            return new PagedResultDto<NotaFiscal>
            {
                Items = items,
                Page = filtro.Page,
                PageSize = filtro.PageSize,
                Total = total
            };
        }
    }
}

