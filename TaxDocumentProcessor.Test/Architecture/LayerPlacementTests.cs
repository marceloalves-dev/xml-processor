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
            var result = Types
                .InAssemblies([
                    ArchitectureConstants.DomainAssembly,
                    ArchitectureConstants.ApplicationAssembly,
                    ArchitectureConstants.InfrastructureAssembly
                ])
                .That()
                .Inherit(typeof(ControllerBase))
                .Should()
                .NotExist()
                .GetResult();

            result.IsSuccessful.Should().BeTrue(
                because: "Controllers must only exist in the API project. Failing types: " +
                         $"{string.Join(", ", result.FailingTypes?.Select(t => t.Name) ?? [])}");
        }

        [Test]
        public void Entities_ShouldResideIn_DomainLayer()
        {
            var result = Types
                .InAssemblies([
                    ArchitectureConstants.ApplicationAssembly,
                    ArchitectureConstants.InfrastructureAssembly,
                    ArchitectureConstants.ApiAssembly
                ])
                .That()
                .ResideInNamespace(ArchitectureConstants.DomainEntitiesNs)
                .Should()
                .NotExist()
                .GetResult();

            result.IsSuccessful.Should().BeTrue(
                because: "Entity types must only reside in Domain. Failing types: " +
                         $"{string.Join(", ", result.FailingTypes?.Select(t => t.Name) ?? [])}");
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
                because: "Only interfaces are allowed in Domain.Repositories. " +
                         "Implementations belong in Infrastructure. Failing types: " +
                         $"{string.Join(", ", result.FailingTypes?.Select(t => t.Name) ?? [])}");
        }

        [Test]
        public void RepositoryImplementations_ShouldResideIn_InfrastructureLayer()
        {
            var result = Types
                .InAssemblies([
                    ArchitectureConstants.DomainAssembly,
                    ArchitectureConstants.ApplicationAssembly,
                    ArchitectureConstants.ApiAssembly
                ])
                .That()
                .ImplementInterface(typeof(INotaFiscalRepository))
                .Should()
                .NotExist()
                .GetResult();

            result.IsSuccessful.Should().BeTrue(
                because: "INotaFiscalRepository implementations must only exist in Infrastructure. Failing types: " +
                         $"{string.Join(", ", result.FailingTypes?.Select(t => t.Name) ?? [])}");
        }

        [Test]
        public void UseCases_ShouldResideIn_ApplicationLayer()
        {
            var result = Types
                .InAssemblies([
                    ArchitectureConstants.DomainAssembly,
                    ArchitectureConstants.InfrastructureAssembly,
                    ArchitectureConstants.ApiAssembly
                ])
                .That()
                .ResideInNamespace(ArchitectureConstants.AppUseCasesNs)
                .Should()
                .NotExist()
                .GetResult();

            result.IsSuccessful.Should().BeTrue(
                because: "Use case types must only reside in Application. Failing types: " +
                         $"{string.Join(", ", result.FailingTypes?.Select(t => t.Name) ?? [])}");
        }
    }
}
