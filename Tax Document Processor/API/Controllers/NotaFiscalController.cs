using Application.DTOs;
using Application.UseCases.NotaFiscalCases;
using Microsoft.AspNetCore.Mvc;
using Tax_Document_Processor.Domain.ValueObjects;

namespace Tax_Document_Processor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotaFiscalController : ControllerBase
    {
        private readonly SaveNotaFiscalUseCase _saveUseCase;
        private readonly GetNotaFiscalByChaveUseCase _getUseCase;
        private readonly DeleteNotaFiscalByChaveUseCase _deleteUseCase;
        private readonly ListNotaFiscalUseCase _listUseCase;

        public NotaFiscalController(
           SaveNotaFiscalUseCase saveUseCase,
           GetNotaFiscalByChaveUseCase getUseCase,
           DeleteNotaFiscalByChaveUseCase deleteUseCase,
           ListNotaFiscalUseCase listUseCase)
        {
            _saveUseCase = saveUseCase;
            _getUseCase = getUseCase;
            _deleteUseCase = deleteUseCase;
            _listUseCase = listUseCase;
        }

        [HttpPost]
        public async Task<IActionResult> SaveAsync([FromBody] string xmlContent)
        {
            var saved = await _saveUseCase.ExecuteAsync(xmlContent);
            return saved ? StatusCode(201) : Ok();
        }

        [HttpGet("{chave}")]
        public async Task<IActionResult> SearchByKeyAsync(string chave)
        {
            var nota = await _getUseCase.ExecuteAsync(new ChaveNota(chave));

            if (nota is null)
                return NotFound();

            return Ok(nota);
        }

        [HttpDelete("{chave}")]
        public async Task<IActionResult> DeleteByKeyAsync(string chave)
        {
            await _deleteUseCase.ExecuteAsync(new ChaveNota(chave));
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync([FromQuery] NotaFiscalFilterDto filtro)
        {
            var notas = await _listUseCase.ExecuteAsync(filtro);
            return Ok(notas);
        }
    }
}