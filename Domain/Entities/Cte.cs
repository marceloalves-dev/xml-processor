using Tax_Document_Processor.Domain.ValueObjects;

namespace Tax_Document_Processor.Domain.Entities
{
    public class Cte : NotaFiscal
    {
        public Cte(
        CnpjOrCpf cnpjEmit,
        CnpjOrCpf cnpjDest,
        string razaoSocial,
        ChaveNota chaveNota,
        string totalValue,
        DateTime dtEmission)
        : base(cnpjEmit, cnpjDest, razaoSocial, chaveNota, totalValue, dtEmission)
        { }

        public override void Validate()
        {
            // regras específicas
        }
    }
}
