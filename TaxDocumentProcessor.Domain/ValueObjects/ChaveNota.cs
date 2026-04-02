namespace TaxDocumentProcessor.Domain.ValueObjects
{
    public class ChaveNota
    {
        public string Value { get; private set; }

        public bool IsNfse => Value.Length == 50;

        public ChaveNota(string value)
        {
            if (value.Length != 44 && value.Length != 50)
                throw new ArgumentException("Chave deve ter 44 ou 50 dígitos");

            Value = value;
        }
    }
}
