using Tax_Document_Processor.Domain.ValueObjects;

namespace Tax_Document_Processor.Domain.Entities
{
    //logica das notas em geral
    public abstract class NotaFiscal
    {

        public Guid Id { get; private set; } = Guid.NewGuid();

        public Cnpj CnpjEmit { get; private set; }

        public Cnpj CnpjDest { get; private set; }

        public string RazaoSocial { get; private set; }

        public ChaveNota ChaveNota { get; private set; }

        public string TotalValue { get; private set; }

        public DateTime DtEmission { get; private set; }

        protected NotaFiscal()
        {

        }

        protected NotaFiscal(
        Cnpj cnpjEmit,
        Cnpj cnpjDest,
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

    }
}
