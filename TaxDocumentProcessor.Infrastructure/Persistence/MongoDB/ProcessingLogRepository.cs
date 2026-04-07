using TaxDocumentProcessor.Infrastructure.Persistence.MongoDB.Entities;

namespace TaxDocumentProcessor.Infrastructure.Persistence.MongoDB
{
    public class ProcessingLogRepository
    {
        private readonly MongoDbContext _context;

        public ProcessingLogRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task InsertAsync(ProcessingLog log, CancellationToken ct = default)
        {
            var collection = _context.GetCollection<ProcessingLog>("processing_logs");
            await collection.InsertOneAsync(log, cancellationToken: ct);
        }
    }
}
