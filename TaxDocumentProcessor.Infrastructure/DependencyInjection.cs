using TaxDocumentProcessor.Infrastructure.Persistence.MongoDB.Maps;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using TaxDocumentProcessor.Application.Services;
using TaxDocumentProcessor.Domain.Repositories;
using TaxDocumentProcessor.Infrastructure.Persistence.MongoDB;
using TaxDocumentProcessor.Infrastructure.XmlLib;

namespace TaxDocumentProcessor.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {

            NotaFiscalMap.Configure();

            // MongoDb
            services.AddSingleton<IMongoClient>(_ =>
                new MongoClient(configuration.GetConnectionString("MongoDB")));

            services.AddSingleton<MongoDbContext>();

            // Repositórios
            services.AddScoped<INotaFiscalRepository, NotaFiscalRepository>();

            // Parser
            services.AddScoped<INotaFiscalParser, NotaFiscalParser>();

            return services;
        }
    }
}
