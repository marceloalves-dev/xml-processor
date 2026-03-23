using Tax_Document_Processor.Domain.ValueObjects;

namespace Application.DTOs
{
    public class NotaFiscalFilterDto
    {
        public DateTime? DtEmission { get; set; }
        public string? RazaoSocial { get; set; }
        public Cnpj? CnpjEmit { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
