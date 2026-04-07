using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using TaxDocumentProcessor.Application.Interfaces;
using TaxDocumentProcessor.Application.Services;
using TaxDocumentProcessor.Domain.Repositories;
using TaxDocumentProcessor.Infrastructure.Messaging;
using TaxDocumentProcessor.Infrastructure.Messaging.Consumers;
using TaxDocumentProcessor.Infrastructure.Persistence.MongoDB;
using TaxDocumentProcessor.Infrastructure.Persistence.MongoDB.Maps;
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
            services.AddScoped<ProcessingLogRepository>();

            // Parser
            services.AddScoped<INotaFiscalParser, NotaFiscalParser>();

            // Mensageria
            services.AddScoped<IEventPublisher, RabbitMqEventPublisher>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<DocumentProcessedConsumer>();

                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configuration["RabbitMQ:Host"], configuration["RabbitMQ:VirtualHost"], h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"]!);
                        h.Password(configuration["RabbitMQ:Password"]!);
                    });

                    cfg.ReceiveEndpoint("document-processed", e =>
                    {
                        e.UseMessageRetry(r => r.Exponential(
                            retryLimit: 3,
                            minInterval: TimeSpan.FromSeconds(1),
                            maxInterval: TimeSpan.FromSeconds(15),
                            intervalDelta: TimeSpan.FromSeconds(2)));

                        e.ConfigureConsumer<DocumentProcessedConsumer>(ctx);
                    });
                });
            });

            return services;
        }
    }
}
