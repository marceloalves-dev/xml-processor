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
    public class GetNotaFiscalByChaveUseCaseTests
    {
        private INotaFiscalRepository _repository;
        private GetNotaFiscalByChaveUseCase _useCase;

        private static readonly ChaveNota ChaveExistente =
            new("12345678901234567890123456789012345678901234");

        [SetUp]
        public void SetUp()
        {
            _repository = Substitute.For<INotaFiscalRepository>();
            _useCase = new GetNotaFiscalByChaveUseCase(_repository);
        }

        [Test]
        public async Task ShouldReturnNotaFiscal_WhenChaveExists()
        {
            // Arrange
            var nota = new Nfe(
                cnpjEmit: new CnpjOrCpf("11222333000181"),
                cnpjDest: new CnpjOrCpf("11222333000181"),
                razaoSocial: "Empresa Teste LTDA",
                chaveNota: ChaveExistente,
                totalValue: "1500.00",
                dtEmission: new DateTime(2025, 1, 10));

            _repository.GetByKeyAsync(ChaveExistente).Returns(nota);

            // Act
            var result = await _useCase.ExecuteAsync(ChaveExistente);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(NotaFiscalResponseDto.From(nota));
        }

        [Test]
        public async Task ShouldReturnNull_WhenChaveDoesNotExist()
        {
            // Arrange
            _repository.GetByKeyAsync(ChaveExistente).Returns((NotaFiscal?)null);

            // Act
            var result = await _useCase.ExecuteAsync(ChaveExistente);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task ShouldCallRepository_WithCorrectChave()
        {
            // Arrange
            _repository.GetByKeyAsync(ChaveExistente).Returns((NotaFiscal?)null);

            // Act
            await _useCase.ExecuteAsync(ChaveExistente);

            // Assert
            await _repository.Received(1).GetByKeyAsync(ChaveExistente);
        }
    }
}
