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

        public async Task<bool> SaveAsync(NotaFiscal nota, CancellationToken cancellationToken = default)
        {
            try
            {
                await _collection.InsertOneAsync(nota, cancellationToken: cancellationToken);
                return true;
            }
            catch (MongoWriteException ex) when (ex.WriteError.Code == 11000)
            {
                return false;
            }
        }

        public async Task<NotaFiscal?> GetByKeyAsync(ChaveNota chave, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(x => x.ChaveNota == chave).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> DeleteAsync(ChaveNota chave, CancellationToken cancellationToken = default)
        {
            var result = await _collection.DeleteOneAsync(x => x.ChaveNota == chave, cancellationToken);
            return result.DeletedCount > 0;
        }

        public async Task<List<NotaFiscal>> ListAsync(
            Expression<Func<NotaFiscal, bool>>? filter = null,
            int page = 1,
            int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            var query = filter is null
                ? _collection.Find(Builders<NotaFiscal>.Filter.Empty)
                : _collection.Find(filter);

            return await query
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<long> CountAsync(Expression<Func<NotaFiscal, bool>>? filter = null,
            CancellationToken cancellationToken = default)
        {
            return filter is null
                ? await _collection.CountDocumentsAsync(Builders<NotaFiscal>.Filter.Empty, cancellationToken: cancellationToken)
                : await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        }

        public async Task UpdateAsync(ChaveNota chave, NotaFiscal nota, CancellationToken cancellationToken = default)
        {
            await _collection.ReplaceOneAsync(x => x.ChaveNota == chave, nota, cancellationToken: cancellationToken);
        }

        public async Task EnsureIndexesAsync()
        {
            var indexModel = new CreateIndexModel<NotaFiscal>(
                Builders<NotaFiscal>.IndexKeys.Ascending("ChaveNota.Value"),
                new CreateIndexOptions { Unique = true });
            await _collection.Indexes.CreateOneAsync(indexModel);
        }
    }
}
