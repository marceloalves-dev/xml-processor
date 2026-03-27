using Tax_Document_Processor.Domain.Entities;

namespace Application.DTOs
{
    public class NotaFiscalResponseDto
    {
        public required string Tipo { get; init; }
        public required string CnpjEmit { get; init; }
        public required string CnpjDest { get; init; }
        public required string RazaoSocial { get; init; }
        public required string ChaveNota { get; init; }
        public required string TotalValue { get; init; }
        public required DateTime DtEmission { get; init; }

        public static NotaFiscalResponseDto From(NotaFiscal nota) => new()
        {
            Tipo = nota switch
            {
                Nfe => "NF-e",
                Cte => "CT-e",
                Nfse => "NFS-e",
                _ => "Unknown"
            },
            CnpjEmit = nota.CnpjEmit.Value,
            CnpjDest = nota.CnpjDest.Value,
            RazaoSocial = nota.RazaoSocial,
            ChaveNota = nota.ChaveNota.Value,
            TotalValue = nota.TotalValue,
            DtEmission = nota.DtEmission
        };
    }
}
