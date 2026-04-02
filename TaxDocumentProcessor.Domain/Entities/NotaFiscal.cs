using TaxDocumentProcessor.Domain.ValueObjects;

namespace TaxDocumentProcessor.Domain.Entities
{
    //logica das notas em geral
    public abstract class NotaFiscal
    {

        public Guid Id { get; private set; } = Guid.NewGuid();

        public CnpjOrCpf CnpjEmit { get; private set; }

        public CnpjOrCpf CnpjDest { get; private set; }

        public string RazaoSocial { get; private set; }

        public ChaveNota ChaveNota { get; private set; }

        public string TotalValue { get; private set; }

        public DateTime DtEmission { get; private set; }

        protected NotaFiscal()
        {

        }

        protected NotaFiscal(
        CnpjOrCpf cnpjEmit,
        CnpjOrCpf cnpjDest,
        string razaoSocial,
        ChaveNota chaveNota,
        string totalValue,
        DateTime dtEmission)
        {
            Id = Guid.NewGuid();
            CnpjEmit = cnpjEmit;
            CnpjDest = cnpjDest;
            RazaoSocial = razaoSocial;
            ChaveNota = chaveNota;
            TotalValue = totalValue;
            DtEmission = dtEmission;
        }


        public abstract void Validate();

        public void Update(string? razaoSocial, string? totalValue)
        {
            if (razaoSocial is not null)
                RazaoSocial = razaoSocial;
            if (totalValue is not null)
                TotalValue = totalValue;
        }

    }
}
