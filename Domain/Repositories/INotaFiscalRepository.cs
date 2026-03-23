using System.Linq.Expressions;
using Tax_Document_Processor.Domain.Entities;
using Tax_Document_Processor.Domain.ValueObjects;

namespace Tax_Document_Processor.Domain.Repositories
{
    public interface INotaFiscalRepository
    {
        Task SaveAsync(NotaFiscal nota);

        Task<NotaFiscal?> GetByKeyAsync(ChaveNota chave);

        Task DeleteAsync(ChaveNota chave);

        Task<IEnumerable<NotaFiscal>> ListAsync(
            Expression<Func<NotaFiscal, bool>>? filter = null,
            int page = 1, int pageSize = 20);

        Task<long> CountAsync(Expression<Func<NotaFiscal, bool>>? filter = null);

    }
}
