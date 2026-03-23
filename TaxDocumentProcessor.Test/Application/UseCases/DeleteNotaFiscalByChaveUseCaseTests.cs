using FluentAssertions;
using NSubstitute;
using Application.UseCases.NotaFiscalCases;
using Tax_Document_Processor.Domain.Entities;
using Tax_Document_Processor.Domain.Repositories;
using Tax_Document_Processor.Domain.ValueObjects;

namespace Tax_Document_Processor.Tests.Application.UseCases
{
    [TestFixture]
    public class DeleteNotaFiscalByChaveUseCaseTests
    {
        private INotaFiscalRepository _repository;
        private DeleteNotaFiscalByChaveUseCase _useCase;

        private static readonly ChaveNota Chave =
            new("12345678901234567890123456789012345678901234");

        [SetUp]
        public void SetUp()
        {
            _repository = Substitute.For<INotaFiscalRepository>();
            _useCase = new DeleteNotaFiscalByChaveUseCase(_repository);
        }

        [Test]
        public async Task ShouldDeleteNotaFiscal_WhenChaveExists()
        {
            // Arrange
            var nota = new Nfe(
                cnpjEmit: new Cnpj("11222333000181"),
                cnpjDest: new Cnpj("11222333000181"),
                razaoSocial: "Empresa Teste LTDA",
                chaveNota: Chave,
                totalValue: "1500.00",
                dtEmission: new DateTime(2025, 1, 10));

            _repository.GetByKeyAsync(Chave).Returns(nota);

            // Act
            await _useCase.ExecuteAsync(Chave);

            // Assert
            await _repository.Received(1).DeleteAsync(Chave);
        }

        [Test]
        public async Task ShouldThrowException_WhenChaveDoesNotExist()
        {
            // Arrange
            _repository.GetByKeyAsync(Chave).Returns((NotaFiscal?)null);

            // Act
            var act = async () => await _useCase.ExecuteAsync(Chave);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Nota fiscal não encontrada");
        }

        [Test]
        public async Task ShouldNotCallDelete_WhenChaveDoesNotExist()
        {
            // Arrange
            _repository.GetByKeyAsync(Chave).Returns((NotaFiscal?)null);

            // Act
            var act = async () => await _useCase.ExecuteAsync(Chave);
            await act.Should().ThrowAsync<Exception>();

            // Assert
            await _repository.DidNotReceive().DeleteAsync(Arg.Any<ChaveNota>());
        }
    }
}
