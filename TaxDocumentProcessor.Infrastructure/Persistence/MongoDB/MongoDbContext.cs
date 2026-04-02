using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace TaxDocumentProcessor.Infrastructure.Persistence.MongoDB
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IMongoClient mongoClient, IConfiguration configuration)
        {
            _database = mongoClient.GetDatabase(configuration["MongoDB:DatabaseName"]);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }

    }
}
