using Application.DTOs;
using MongoDB.Driver;
using System.Linq.Expressions;
using Tax_Document_Processor.Domain.Entities;
using Tax_Document_Processor.Domain.Repositories;
using Tax_Document_Processor.Domain.ValueObjects;

namespace Tax_Document_Processor.Infrastructure.Persistence.MongoDB
{
    public class NotaFiscalRepository : INotaFiscalRepository
    {
        private readonly IMongoCollection<NotaFiscal> _collection;

        public NotaFiscalRepository(MongoDbContext context)
        {
            _collection = context.GetCollection<NotaFiscal>("notas_fiscais");
        }

        public async Task SaveAsync(NotaFiscal nota)
        {
            await _collection.InsertOneAsync(nota);
        }

        public async Task<NotaFiscal?> GetByKeyAsync(ChaveNota chave)
        {
            return await _collection.Find(x => x.ChaveNota == chave).FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(ChaveNota chave)
        {
            await _collection.DeleteOneAsync(x => x.ChaveNota == chave);
        }

        public async Task<IEnumerable<NotaFiscal>> ListAsync(
        Expression<Func<NotaFiscal, bool>>? filter = null,
        int page = 1,
        int pageSize = 20)
        {
            var query = filter is null
                ? _collection.Find(Builders<NotaFiscal>.Filter.Empty)
                : _collection.Find(filter);

            return await query
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task<long> CountAsync(Expression<Func<NotaFiscal, bool>>? filter = null)
        {
            return filter is null
                ? await _collection.CountDocumentsAsync(Builders<NotaFiscal>.Filter.Empty)
                : await _collection.CountDocumentsAsync(filter);
        }
    }
}
