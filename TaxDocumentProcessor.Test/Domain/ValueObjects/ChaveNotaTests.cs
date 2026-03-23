using FluentAssertions;
using Tax_Document_Processor.Domain.ValueObjects;

namespace Tax_Document_Processor.Tests.Domain.ValueObjects
{
    [TestFixture]
    public class ChaveNotaTests
    {
        [TestCase("12345678901234567890123456789012345678901234")]
        public void ShouldCreateChaveNota_WhenValueIsValid(string value)
        {
            // Act
            var act = () => new ChaveNota(value);

            // Assert
            act.Should().NotThrow();
        }

        [TestCase("1234567890123456789012345678901234567890123")]
        [TestCase("123456789012345678901234567890123456789012345")]
        [TestCase("")]
        public void ShouldThrowException_WhenValueIsInvalid(string value)
        {
            // Act
            var act = () => new ChaveNota(value);

            // Assert
            act.Should().Throw<Exception>()
                .WithMessage("Chave deve ter 44 dígitos");
        }
    }
}