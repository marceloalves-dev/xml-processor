using FluentAssertions;
using Tax_Document_Processor.Domain.ValueObjects;

namespace Tax_Document_Processor.Tests.Domain.ValueObjects
{
    [TestFixture]
    public class CnpjOrCpfTests
    {
        [TestCase("11222333000181")]
        [TestCase("11.222.333/0001-81")]
        [TestCase("30575868805")]
        [TestCase("305.758.688-05")]
        public void ShouldCreate_WhenValueIsValid(string value)
        {
            // Act
            var act = () => new CnpjOrCpf(value);

            // Assert
            act.Should().NotThrow();
        }

        [TestCase("00000000000000")]
        [TestCase("11111111111111")]
        [TestCase("00000000000")]
        [TestCase("11111111111")]
        [TestCase("1234567890123")]
        [TestCase("")]
        [TestCase("abcdefghijklmn")]
        public void ShouldThrowException_WhenValueIsInvalid(string value)
        {
            // Act
            var act = () => new CnpjOrCpf(value);

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("CNPJ ou CPF inválido.");
        }

        [TestCase("30575868805")]
        [TestCase("305.758.688-05")]
        public void ShouldSetIsCpfTrue_WhenValueIsCpf(string value)
        {
            // Act
            var doc = new CnpjOrCpf(value);

            // Assert
            doc.IsCpf.Should().BeTrue();
        }

        [TestCase("11222333000181")]
        [TestCase("11.222.333/0001-81")]
        public void ShouldSetIsCpfFalse_WhenValueIsCnpj(string value)
        {
            // Act
            var doc = new CnpjOrCpf(value);

            // Assert
            doc.IsCpf.Should().BeFalse();
        }

        [TestCase("305.758.688-05", "30575868805")]
        [TestCase("11.222.333/0001-81", "11222333000181")]
        public void ShouldStoreCleanedValue(string input, string expected)
        {
            // Act
            var doc = new CnpjOrCpf(input);

            // Assert
            doc.Value.Should().Be(expected);
        }
    }
}
