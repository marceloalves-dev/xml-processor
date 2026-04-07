using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaxDocumentProcessor.Application.DTOs;
using TaxDocumentProcessor.Application.UseCases.NotaFiscalCases;
using TaxDocumentProcessor.Domain.ValueObjects;

namespace TaxDocumentProcessor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotaFiscalController : ControllerBase
    {
        private readonly SaveNotaFiscalUseCase _saveUseCase;
        private readonly GetNotaFiscalByChaveUseCase _getUseCase;
        private readonly DeleteNotaFiscalByChaveUseCase _deleteUseCase;
        private readonly ListNotaFiscalUseCase _listUseCase;
        private readonly UpdateNotaFiscalUseCase _updateUseCase;

        public NotaFiscalController(
           SaveNotaFiscalUseCase saveUseCase,
           GetNotaFiscalByChaveUseCase getUseCase,
           DeleteNotaFiscalByChaveUseCase deleteUseCase,
           ListNotaFiscalUseCase listUseCase,
           UpdateNotaFiscalUseCase updateUseCase)
        {
            _saveUseCase = saveUseCase;
            _getUseCase = getUseCase;
            _deleteUseCase = deleteUseCase;
            _listUseCase = listUseCase;
            _updateUseCase = updateUseCase;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SaveAsync([FromForm] string xmlContent, CancellationToken cancellationToken)
        {
            var saved = await _saveUseCase.ExecuteAsync(xmlContent, cancellationToken);
            return saved ? StatusCode(201) : Ok();
        }

        [HttpGet("{chave}")]
        public async Task<IActionResult> SearchByKeyAsync(string chave, CancellationToken cancellationToken)
        {
            var nota = await _getUseCase.ExecuteAsync(new ChaveNota(chave), cancellationToken);

            if (nota is null)
                return NotFound();

            return Ok(nota);
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync([FromQuery] NotaFiscalFilterDto filtro, CancellationToken cancellationToken)
        {
            var notas = await _listUseCase.ExecuteAsync(filtro, cancellationToken);
            return Ok(notas);
        }

        [HttpPut("{chave}")]
        public async Task<IActionResult> UpdateAsync(string chave, [FromBody] UpdateNotaFiscalRequest request, CancellationToken cancellationToken)
        {
            var nota = await _updateUseCase.ExecuteAsync(new ChaveNota(chave), request, cancellationToken);

            if (nota is null)
                return NotFound();

            return Ok(nota);
        }

        [HttpDelete("{chave}")]
        public async Task<IActionResult> DeleteByKeyAsync(string chave, CancellationToken cancellationToken)
        {
            await _deleteUseCase.ExecuteAsync(new ChaveNota(chave), cancellationToken);
            return NoContent();
        }
        
    }
}
