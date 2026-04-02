using TaxDocumentProcessor.Domain.ValueObjects;

namespace TaxDocumentProcessor.Domain.Entities
{
    public class Nfe : NotaFiscal
    {
        public Nfe(
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
