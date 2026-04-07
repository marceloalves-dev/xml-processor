using NetArchTest.Rules;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using TaxDocumentProcessor.Domain.Repositories;

namespace TaxDocumentProcessor.Tests.Architecture
{
    [TestFixture]
    public class LayerPlacementTests
    {
        [Test]
        public void Controllers_ShouldResideOnly_InApiLayer()
        {
            var types = Types
                .InAssemblies([
                    ArchitectureConstants.DomainAssembly,
                    ArchitectureConstants.ApplicationAssembly,
                    ArchitectureConstants.InfrastructureAssembly
                ])
                .That()
                .Inherit(typeof(ControllerBase))
                .GetTypes();

            types.Should().BeEmpty(because: "controllers must only exist in the API project");
        }

        [Test]
        public void Entities_ShouldResideIn_DomainLayer()
        {
            var types = Types
                .InAssemblies([
                    ArchitectureConstants.ApplicationAssembly,
                    ArchitectureConstants.InfrastructureAssembly,
                    ArchitectureConstants.ApiAssembly
                ])
                .That()
                .ResideInNamespace(ArchitectureConstants.DomainEntitiesNs)
                .GetTypes();

            types.Should().BeEmpty(because: "entity types must only reside in Domain");
        }

        [Test]
        public void RepositoryInterfaces_ShouldResideIn_DomainLayer()
        {
            var result = Types
                .InAssembly(ArchitectureConstants.DomainAssembly)
                .That()
                .ResideInNamespace(ArchitectureConstants.DomainRepositoriesNs)
                .Should()
                .BeInterfaces()
                .GetResult();

            result.IsSuccessful.Should().BeTrue(
                because: "only interfaces are allowed in Domain.Repositories — implementations belong in Infrastructure. Failing types: " +
                         $"{string.Join(", ", result.FailingTypes?.Select(t => t.Name) ?? [])}");
        }

        [Test]
        public void RepositoryImplementations_ShouldResideIn_InfrastructureLayer()
        {
            var types = Types
                .InAssemblies([
                    ArchitectureConstants.DomainAssembly,
                    ArchitectureConstants.ApplicationAssembly,
                    ArchitectureConstants.ApiAssembly
                ])
                .That()
                .ImplementInterface(typeof(INotaFiscalRepository))
                .GetTypes();

            types.Should().BeEmpty(because: "INotaFiscalRepository implementations must only exist in Infrastructure");
        }

        [Test]
        public void UseCases_ShouldResideIn_ApplicationLayer()
        {
            var types = Types
                .InAssemblies([
                    ArchitectureConstants.DomainAssembly,
                    ArchitectureConstants.InfrastructureAssembly,
                    ArchitectureConstants.ApiAssembly
                ])
                .That()
                .ResideInNamespace(ArchitectureConstants.AppUseCasesNs)
                .GetTypes();

            types.Should().BeEmpty(because: "use case types must only reside in Application");
        }
    }
}
