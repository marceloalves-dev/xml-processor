using FluentAssertions;
using NSubstitute;
using Application.UseCases.NotaFiscalCases;
using Tax_Document_Processor.Application.Services;
using Tax_Document_Processor.Domain.Entities;
using Tax_Document_Processor.Domain.Repositories;
using Tax_Document_Processor.Domain.ValueObjects;

namespace Tax_Document_Processor.Tests.Application.UseCases
{
    [TestFixture]
    public class SaveNotaFiscalUseCaseTests
    {
        private INotaFiscalRepository _repository;
        private INotaFiscalParser _parser;
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
            _useCase = new SaveNotaFiscalUseCase(_repository, _parser);
        }

        [Test]
        public async Task ShouldSaveNotaFiscal_AndReturnTrue_WhenItDoesNotExist()
        {
            // Arrange
            var nota = CreateNota();
            _parser.Parse(XmlContent).Returns(nota);
            _repository.GetByKeyAsync(Chave).Returns((NotaFiscal?)null);

            // Act
            var result = await _useCase.ExecuteAsync(XmlContent);

            // Assert
            await _repository.Received(1).SaveAsync(nota);
            result.Should().BeTrue();
        }

        [Test]
        public async Task ShouldNotSave_AndReturnFalse_WhenNotaAlreadyExists()
        {
            // Arrange
            var nota = CreateNota();
            _parser.Parse(XmlContent).Returns(nota);
            _repository.GetByKeyAsync(Chave).Returns(nota);

            // Act
            var result = await _useCase.ExecuteAsync(XmlContent);

            // Assert
            await _repository.DidNotReceive().SaveAsync(Arg.Any<NotaFiscal>());
            result.Should().BeFalse();
        }

        [Test]
        public async Task ShouldCallParser_WithCorrectXml()
        {
            // Arrange
            var nota = CreateNota();
            _parser.Parse(XmlContent).Returns(nota);
            _repository.GetByKeyAsync(Chave).Returns((NotaFiscal?)null);

            // Act
            await _useCase.ExecuteAsync(XmlContent);

            // Assert
            _parser.Received(1).Parse(XmlContent);
        }
    }
}
