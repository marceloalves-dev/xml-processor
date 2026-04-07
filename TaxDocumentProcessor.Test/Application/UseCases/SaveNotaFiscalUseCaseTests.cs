using FluentAssertions;
using NSubstitute;
using TaxDocumentProcessor.Application.Interfaces;
using TaxDocumentProcessor.Application.UseCases.NotaFiscalCases;
using TaxDocumentProcessor.Application.Services;
using TaxDocumentProcessor.Domain.Entities;
using TaxDocumentProcessor.Domain.Repositories;
using TaxDocumentProcessor.Domain.ValueObjects;

namespace TaxDocumentProcessor.Tests.Application.UseCases
{
    [TestFixture]
    public class SaveNotaFiscalUseCaseTests
    {
        private INotaFiscalRepository _repository;
        private INotaFiscalParser _parser;
        private IEventPublisher _eventPublisher;
        private SaveNotaFiscalUseCase _useCase;

        private const string XmlContent = "<nfe>conteudo</nfe>";

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
            _parser = Substitute.For<INotaFiscalParser>();
            _eventPublisher = Substitute.For<IEventPublisher>();
            _useCase = new SaveNotaFiscalUseCase(_repository, _parser, _eventPublisher);
        }

        [Test]
        public async Task ShouldReturnTrue_WhenSaveSucceeds()
        {
            // Arrange
            var nota = CreateNota();
            _parser.Parse(XmlContent).Returns(nota);
            _repository.SaveAsync(nota).Returns(true);

            // Act
            var result = await _useCase.ExecuteAsync(XmlContent);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public async Task ShouldReturnFalse_WhenDocumentAlreadyExists()
        {
            // Arrange
            var nota = CreateNota();
            _parser.Parse(XmlContent).Returns(nota);
            _repository.SaveAsync(nota).Returns(false);

            // Act
            var result = await _useCase.ExecuteAsync(XmlContent);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public async Task ShouldCallSaveAsync_WithParsedNota()
        {
            // Arrange
            var nota = CreateNota();
            _parser.Parse(XmlContent).Returns(nota);
            _repository.SaveAsync(nota).Returns(true);

            // Act
            await _useCase.ExecuteAsync(XmlContent);

            // Assert
            await _repository.Received(1).SaveAsync(nota);
        }

        [Test]
        public async Task ShouldCallParser_WithCorrectXml()
        {
            // Arrange
            var nota = CreateNota();
            _parser.Parse(XmlContent).Returns(nota);
            _repository.SaveAsync(nota).Returns(true);

            // Act
            await _useCase.ExecuteAsync(XmlContent);

            // Assert
            _parser.Received(1).Parse(XmlContent);
        }

        [Test]
        public async Task ShouldNotCallGetByKeyAsync()
        {
            // Arrange
            var nota = CreateNota();
            _parser.Parse(XmlContent).Returns(nota);
            _repository.SaveAsync(nota).Returns(true);

            // Act
            await _useCase.ExecuteAsync(XmlContent);

            // Assert — N+1 eliminado: não faz GET antes do INSERT
            await _repository.DidNotReceive().GetByKeyAsync(Arg.Any<ChaveNota>());
        }
    }
}
