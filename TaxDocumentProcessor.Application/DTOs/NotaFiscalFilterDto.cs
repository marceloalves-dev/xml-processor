using TaxDocumentProcessor.Domain.ValueObjects;

namespace TaxDocumentProcessor.Application.DTOs
{
    public class NotaFiscalFilterDto
    {
        public DateTime? DtEmission { get; set; }
        public string? RazaoSocial { get; set; }
        public CnpjOrCpf? CnpjEmit { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
