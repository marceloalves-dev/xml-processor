using System.Reflection;
using TaxDocumentProcessor.Domain.Entities;
using TaxDocumentProcessor.Application.UseCases.NotaFiscalCases;
using TaxDocumentProcessor.Infrastructure.Persistence.MongoDB;
using TaxDocumentProcessor.API.Controllers;

namespace TaxDocumentProcessor.Tests.Architecture
{
    internal static class ArchitectureConstants
    {
        internal static readonly Assembly DomainAssembly =
            typeof(NotaFiscal).Assembly;

        internal static readonly Assembly ApplicationAssembly =
            typeof(SaveNotaFiscalUseCase).Assembly;

        internal static readonly Assembly InfrastructureAssembly =
            typeof(NotaFiscalRepository).Assembly;

        internal static readonly Assembly ApiAssembly =
            typeof(NotaFiscalController).Assembly;

        internal const string DomainNs         = "TaxDocumentProcessor.Domain";
        internal const string ApplicationNs    = "TaxDocumentProcessor.Application";
        internal const string InfrastructureNs = "TaxDocumentProcessor.Infrastructure";
        internal const string ApiNs            = "TaxDocumentProcessor.API";

        internal const string DomainEntitiesNs     = "TaxDocumentProcessor.Domain.Entities";
        internal const string DomainValueObjectsNs  = "TaxDocumentProcessor.Domain.ValueObjects";
        internal const string DomainRepositoriesNs  = "TaxDocumentProcessor.Domain.Repositories";
        internal const string AppUseCasesNs         = "TaxDocumentProcessor.Application.UseCases";
        internal const string InfraPersistenceNs    = "TaxDocumentProcessor.Infrastructure.Persistence.MongoDB";
        internal const string ApiControllersNs      = "TaxDocumentProcessor.API.Controllers";
    }
}
