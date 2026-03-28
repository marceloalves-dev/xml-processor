using FluentAssertions;
using NSubstitute;
using Application.DTOs;
using Application.UseCases.NotaFiscalCases;
using Tax_Document_Processor.Domain.Entities;
using Tax_Document_Processor.Domain.Repositories;
using Tax_Document_Processor.Domain.ValueObjects;

namespace Tax_Document_Processor.Tests.Application.UseCases
{
    [TestFixture]
    public class UpdateNotaFiscalUseCaseTests
    {
        private INotaFiscalRepository _repository;
        private UpdateNotaFiscalUseCase _useCase;

        private static readonly ChaveNota Chave =
            new("12345678901234567890123456789012345678901234");

        private static Nfe CreateNota() => new(
            cnpjEmit: new CnpjOrCpf("11222333000181"),
            cnpjDest: new CnpjOrCpf("11222333000181"),
            razaoSocial: "Empresa Teste LTDA",
            chaveNota: Chave,
            totalValue: "1500.00",
            dtEmission: new DateTime(2025, 1, 10));

        [SetUp]
        public void SetUp()
        {
            _repository = Substitute.For<INotaFiscalRepository>();
            _useCase = new UpdateNotaFiscalUseCase(_repository);
        }

        [Test]
        public async Task ShouldReturnNull_WhenChaveDoesNotExist()
        {
            // Arrange
            _repository.GetByKeyAsync(Chave).Returns((NotaFiscal?)null);
            var request = new UpdateNotaFiscalRequest { RazaoSocial = "Nova Empresa" };

            // Act
            var result = await _useCase.ExecuteAsync(Chave, request);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task ShouldNotCallUpdateAsync_WhenChaveDoesNotExist()
        {
            // Arrange
            _repository.GetByKeyAsync(Chave).Returns((NotaFiscal?)null);
            var request = new UpdateNotaFiscalRequest { RazaoSocial = "Nova Empresa" };

            // Act
            await _useCase.ExecuteAsync(Chave, request);

            // Assert
            await _repository.DidNotReceive().UpdateAsync(Arg.Any<ChaveNota>(), Arg.Any<NotaFiscal>());
        }

        [Test]
        public async Task ShouldReturnUpdatedNota_WhenChaveExists()
        {
            // Arrange
            var nota = CreateNota();
            _repository.GetByKeyAsync(Chave).Returns(nota);
            var request = new UpdateNotaFiscalRequest { RazaoSocial = "Nova Empresa", TotalValue = "2000.00" };

            // Act
            var result = await _useCase.ExecuteAsync(Chave, request);

            // Assert
            result.Should().NotBeNull();
            result!.RazaoSocial.Should().Be("Nova Empresa");
            result!.TotalValue.Should().Be("2000.00");
        }

        [Test]
        public async Task ShouldCallUpdateAsync_WhenChaveExists()
        {
            // Arrange
            var nota = CreateNota();
            _repository.GetByKeyAsync(Chave).Returns(nota);
            var request = new UpdateNotaFiscalRequest { RazaoSocial = "Nova Empresa" };

            // Act
            await _useCase.ExecuteAsync(Chave, request);

            // Assert
            await _repository.Received(1).UpdateAsync(Chave, nota);
        }

        [Test]
        public async Task ShouldUpdateRazaoSocial_WhenProvided()
        {
            // Arrange
            var nota = CreateNota();
            _repository.GetByKeyAsync(Chave).Returns(nota);
            var request = new UpdateNotaFiscalRequest { RazaoSocial = "Novo Nome LTDA" };

            // Act
            var result = await _useCase.ExecuteAsync(Chave, request);

            // Assert
            result!.RazaoSocial.Should().Be("Novo Nome LTDA");
        }

        [Test]
        public async Task ShouldUpdateTotalValue_WhenProvided()
        {
            // Arrange
            var nota = CreateNota();
            _repository.GetByKeyAsync(Chave).Returns(nota);
            var request = new UpdateNotaFiscalRequest { TotalValue = "9999.99" };

            // Act
            var result = await _useCase.ExecuteAsync(Chave, request);

            // Assert
            result!.TotalValue.Should().Be("9999.99");
        }

        [Test]
        public async Task ShouldKeepOriginalRazaoSocial_WhenNotProvided()
        {
            // Arrange
            var nota = CreateNota();
            _repository.GetByKeyAsync(Chave).Returns(nota);
            var request = new UpdateNotaFiscalRequest { TotalValue = "2000.00" };

            // Act
            var result = await _useCase.ExecuteAsync(Chave, request);

            // Assert
            result!.RazaoSocial.Should().Be("Empresa Teste LTDA");
        }

        [Test]
        public async Task ShouldKeepOriginalTotalValue_WhenNotProvided()
        {
            // Arrange
            var nota = CreateNota();
            _repository.GetByKeyAsync(Chave).Returns(nota);
            var request = new UpdateNotaFiscalRequest { RazaoSocial = "Novo Nome LTDA" };

            // Act
            var result = await _useCase.ExecuteAsync(Chave, request);

            // Assert
            result!.TotalValue.Should().Be("1500.00");
        }

        [Test]
        public async Task ShouldCallGetByKeyAsync_WithCorrectChave()
        {
            // Arrange
            _repository.GetByKeyAsync(Chave).Returns((NotaFiscal?)null);
            var request = new UpdateNotaFiscalRequest { RazaoSocial = "Nova Empresa" };

            // Act
            await _useCase.ExecuteAsync(Chave, request);

            // Assert
            await _repository.Received(1).GetByKeyAsync(Chave);
        }
    }
}
