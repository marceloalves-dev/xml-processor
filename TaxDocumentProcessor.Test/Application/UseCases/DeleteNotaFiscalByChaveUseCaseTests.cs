using FluentAssertions;
using NSubstitute;
using TaxDocumentProcessor.Application.UseCases.NotaFiscalCases;
using TaxDocumentProcessor.Domain.Entities;
using TaxDocumentProcessor.Domain.Repositories;
using TaxDocumentProcessor.Domain.ValueObjects;

namespace TaxDocumentProcessor.Tests.Application.UseCases
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
        public async Task ShouldCallDeleteAsync_WithCorrectChave()
        {
            // Arrange
            _repository.DeleteAsync(Chave).Returns(true);

            // Act
            await _useCase.ExecuteAsync(Chave);

            // Assert
            await _repository.Received(1).DeleteAsync(Chave);
        }

        [Test]
        public async Task ShouldThrowException_WhenChaveDoesNotExist()
        {
            // Arrange
            _repository.DeleteAsync(Chave).Returns(false);

            // Act
            var act = async () => await _useCase.ExecuteAsync(Chave);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Nota fiscal não encontrada");
        }

        [Test]
        public async Task ShouldNotCallGetByKeyAsync()
        {
            // Arrange
            _repository.DeleteAsync(Chave).Returns(true);

            // Act
            await _useCase.ExecuteAsync(Chave);

            // Assert — N+1 eliminado: não faz GET antes do DELETE
            await _repository.DidNotReceive().GetByKeyAsync(Arg.Any<ChaveNota>());
        }
    }
}
