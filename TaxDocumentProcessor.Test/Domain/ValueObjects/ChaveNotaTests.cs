using FluentAssertions;
using TaxDocumentProcessor.Domain.ValueObjects;

namespace TaxDocumentProcessor.Tests.Domain.ValueObjects
{
    [TestFixture]
    public class ChaveNotaTests
    {
        [TestCase("12345678901234567890123456789012345678901234")]       // 44 - NF-e/CT-e
        [TestCase("12345678901234567890123456789012345678901234567890")] // 50 - NFS-e
        public void ShouldCreateChaveNota_WhenValueIsValid(string value)
        {
            // Act
            var act = () => new ChaveNota(value);

            // Assert
            act.Should().NotThrow();
        }

        [TestCase("1234567890123456789012345678901234567890123")]          // 43
        [TestCase("123456789012345678901234567890123456789012345")]         // 45
        [TestCase("123456789012345678901234567890123456789012345678")]      // 48
        [TestCase("123456789012345678901234567890123456789012345678901")]   // 51
        [TestCase("")]
        public void ShouldThrowException_WhenValueIsInvalid(string value)
        {
            // Act
            var act = () => new ChaveNota(value);

            // Assert
            act.Should().Throw<Exception>()
                .WithMessage("Chave deve ter 44 ou 50 dígitos");
        }

        [Test]
        public void ShouldSetIsNfseTrue_WhenValueHas50Digits()
        {
            var chave = new ChaveNota("12345678901234567890123456789012345678901234567890");

            chave.IsNfse.Should().BeTrue();
        }

        [Test]
        public void ShouldSetIsNfseFalse_WhenValueHas44Digits()
        {
            var chave = new ChaveNota("12345678901234567890123456789012345678901234");

            chave.IsNfse.Should().BeFalse();
        }
    }
}