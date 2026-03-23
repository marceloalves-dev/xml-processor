using Infrastructure.Persistence.MongoDB.Maps;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Tax_Document_Processor.Application.Services;
using Tax_Document_Processor.Domain.Repositories;
using Tax_Document_Processor.Infrastructure.Persistence.MongoDB;
using Tax_Document_Processor.Infrastructure.XmlLib;

namespace Tax_Document_Processor.Infrastructure
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
