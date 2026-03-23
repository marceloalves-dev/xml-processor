using FluentAssertions;
using Tax_Document_Processor.Domain.ValueObjects;

namespace Tax_Document_Processor.Tests.Domain.ValueObjects
{
    [TestFixture]
    public class CnpjTests
    {
        [TestCase("11222333000181")]
        [TestCase("11.222.333/0001-81")]
        [TestCase("62.797.508/0001-10")]
        public void ShouldCreateCnpj_WhenValueIsValid(string value)
        {
            // Act
            var act = () => new Cnpj(value);

            // Assert
            act.Should().NotThrow();
        }

        [TestCase("00000000000000")]
        [TestCase("11111111111111")]
        [TestCase("1234567890123")]
        [TestCase("123456789012345")]
        [TestCase("11.222.333/0001-00")]
        [TestCase("")]
        [TestCase("abcdefghijklmn")]
        public void ShouldThrowException_WhenValueIsInvalid(string value)
        {
            // Act
            var act = () => new Cnpj(value);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("CNPJ inválido.");
        }
    }
}