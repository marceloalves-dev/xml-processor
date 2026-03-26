using System.Linq.Expressions;
using Tax_Document_Processor.Domain.Entities;
using Tax_Document_Processor.Domain.ValueObjects;

namespace Tax_Document_Processor.Domain.Repositories
{
    public interface INotaFiscalRepository
    {
        /// <returns>true if inserted, false if already existed (duplicate key)</returns>
        Task<bool> SaveAsync(NotaFiscal nota, CancellationToken cancellationToken = default);

        Task<NotaFiscal?> GetByKeyAsync(ChaveNota chave, CancellationToken cancellationToken = default);

        /// <returns>true if deleted, false if not found</returns>
        Task<bool> DeleteAsync(ChaveNota chave, CancellationToken cancellationToken = default);

        Task<List<NotaFiscal>> ListAsync(
            Expression<Func<NotaFiscal, bool>>? filter = null,
            int page = 1, int pageSize = 20,
            CancellationToken cancellationToken = default);

        Task<long> CountAsync(Expression<Func<NotaFiscal, bool>>? filter = null,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(ChaveNota chave, NotaFiscal nota, CancellationToken cancellationToken = default);

        Task EnsureIndexesAsync();
    }
}
