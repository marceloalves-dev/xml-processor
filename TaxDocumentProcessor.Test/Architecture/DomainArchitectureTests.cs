using NetArchTest.Rules;
using FluentAssertions;

namespace TaxDocumentProcessor.Tests.Architecture
{
    [TestFixture]
    public class DomainArchitectureTests
    {
        [Test]
        public void Domain_ShouldNotDependOn_Application()
        {
            var result = Types
                .InAssembly(ArchitectureConstants.DomainAssembly)
                .Should()
                .NotHaveDependencyOn(ArchitectureConstants.ApplicationNs)
                .GetResult();

            result.IsSuccessful.Should().BeTrue(
                because: "Domain must not reference Application. Failing types: " +
                         $"{string.Join(", ", result.FailingTypes?.Select(t => t.Name) ?? [])}");
        }

        [Test]
        public void Domain_ShouldNotDependOn_Infrastructure()
        {
            var result = Types
                .InAssembly(ArchitectureConstants.DomainAssembly)
                .Should()
                .NotHaveDependencyOn(ArchitectureConstants.InfrastructureNs)
                .GetResult();

            result.IsSuccessful.Should().BeTrue(
                because: "Domain must not reference Infrastructure. Failing types: " +
                         $"{string.Join(", ", result.FailingTypes?.Select(t => t.Name) ?? [])}");
        }

        [Test]
        public void Domain_ShouldNotDependOn_Api()
        {
            var result = Types
                .InAssembly(ArchitectureConstants.DomainAssembly)
                .Should()
                .NotHaveDependencyOn(ArchitectureConstants.ApiNs)
                .GetResult();

            result.IsSuccessful.Should().BeTrue(
                because: "Domain must not reference API. Failing types: " +
                         $"{string.Join(", ", result.FailingTypes?.Select(t => t.Name) ?? [])}");
        }
    }
}
