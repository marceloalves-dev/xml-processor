using NetArchTest.Rules;
using FluentAssertions;

namespace TaxDocumentProcessor.Tests.Architecture
{
    [TestFixture]
    public class InfrastructureArchitectureTests
    {
        [Test]
        public void Infrastructure_ShouldNotDependOn_Api()
        {
            var result = Types
                .InAssembly(ArchitectureConstants.InfrastructureAssembly)
                .Should()
                .NotHaveDependencyOn(ArchitectureConstants.ApiNs)
                .GetResult();

            result.IsSuccessful.Should().BeTrue(
                because: "Infrastructure must not reference API. Failing types: " +
                         $"{string.Join(", ", result.FailingTypes?.Select(t => t.Name) ?? [])}");
        }
    }
}
