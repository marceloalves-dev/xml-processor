using NetArchTest.Rules;
using FluentAssertions;

namespace TaxDocumentProcessor.Tests.Architecture
{
    [TestFixture]
    public class ApplicationArchitectureTests
    {
        [Test]
        public void Application_ShouldNotDependOn_Infrastructure()
        {
            var result = Types
                .InAssembly(ArchitectureConstants.ApplicationAssembly)
                .Should()
                .NotHaveDependencyOn(ArchitectureConstants.InfrastructureNs)
                .GetResult();

            result.IsSuccessful.Should().BeTrue(
                because: "Application must not reference Infrastructure. Failing types: " +
                         $"{string.Join(", ", result.FailingTypes?.Select(t => t.Name) ?? [])}");
        }

        [Test]
        public void Application_ShouldNotDependOn_Api()
        {
            var result = Types
                .InAssembly(ArchitectureConstants.ApplicationAssembly)
                .Should()
                .NotHaveDependencyOn(ArchitectureConstants.ApiNs)
                .GetResult();

            result.IsSuccessful.Should().BeTrue(
                because: "Application must not reference API. Failing types: " +
                         $"{string.Join(", ", result.FailingTypes?.Select(t => t.Name) ?? [])}");
        }
    }
}
