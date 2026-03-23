using Application.DTOs;
using Application.UseCases.NotaFiscalCases;
using FluentAssertions;
using NSubstitute;
using System.Linq.Expressions;
using Tax_Document_Processor.Domain.Entities;
using Tax_Document_Processor.Domain.Repositories;
using Tax_Document_Processor.Domain.ValueObjects;

namespace Tax_Document_Processor.Tests.Application.UseCases
{
    [TestFixture]
    public class ListNotaFiscalUseCaseTests
    {
        private INotaFiscalRepository _repository;
        private ListNotaFiscalUseCase _useCase;

        private static readonly ChaveNota Chave =
            new("12345678901234567890123456789012345678901234");

        private static Nfe CreateNota() => new(
            cnpjEmit: new Cnpj("11222333000181"),
            cnpjDest: new Cnpj("11222333000181"),
            razaoSocial: "Empresa Teste LTDA",
            chaveNota: Chave,
            totalValue: "1500.00",
            dtEmission: new DateTime(2025, 1, 10));

        [SetUp]
        public void SetUp()
        {
            _repository = Substitute.For<INotaFiscalRepository>();
            _useCase = new ListNotaFiscalUseCase(_repository);
        }

        [Test]
        public async Task ShouldReturnPagedResult_WithCorrectValues()
        {
            // Arrange
            var notas = new List<NotaFiscal> { CreateNota() };
            var filtro = new NotaFiscalFilterDto { Page = 2, PageSize = 10 };

            _repository
                .ListAsync(Arg.Any<Expression<Func<NotaFiscal, bool>>>(), filtro.Page, filtro.PageSize)
                .Returns(notas);
            _repository
                .CountAsync(Arg.Any<Expression<Func<NotaFiscal, bool>>>())
                .Returns(42);

            // Act
            var result = await _useCase.ExecuteAsync(filtro);

            // Assert
            result.Items.Should().BeEquivalentTo(notas);
            result.Page.Should().Be(2);
            result.PageSize.Should().Be(10);
            result.Total.Should().Be(42);
        }

        [Test]
        public async Task ShouldCallListAsync_WithCorrectPagination()
        {
            // Arrange
            var filtro = new NotaFiscalFilterDto { Page = 3, PageSize = 5 };

            _repository
                .ListAsync(Arg.Any<Expression<Func<NotaFiscal, bool>>>(), Arg.Any<int>(), Arg.Any<int>())
                .Returns(Enumerable.Empty<NotaFiscal>());
            _repository
                .CountAsync(Arg.Any<Expression<Func<NotaFiscal, bool>>>())
                .Returns(0);

            // Act
            await _useCase.ExecuteAsync(filtro);

            // Assert
            await _repository.Received(1).ListAsync(
                Arg.Any<Expression<Func<NotaFiscal, bool>>>(),
                filtro.Page,
                filtro.PageSize);
        }

        [Test]
        public async Task ShouldCallCountAsync_OncePerExecution()
        {
            // Arrange
            var filtro = new NotaFiscalFilterDto();

            _repository
                .ListAsync(Arg.Any<Expression<Func<NotaFiscal, bool>>>(), Arg.Any<int>(), Arg.Any<int>())
                .Returns(Enumerable.Empty<NotaFiscal>());
            _repository
                .CountAsync(Arg.Any<Expression<Func<NotaFiscal, bool>>>())
                .Returns(0);

            // Act
            await _useCase.ExecuteAsync(filtro);

            // Assert
            await _repository.Received(1).CountAsync(Arg.Any<Expression<Func<NotaFiscal, bool>>>());
        }
    }
}
