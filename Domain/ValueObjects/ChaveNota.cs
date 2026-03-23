namespace Tax_Document_Processor.Domain.ValueObjects
{
    public class ChaveNota
    {
        public string Value { get; private set; }

        public ChaveNota(string value)
        {
            if (value.Length != 44)
                throw new Exception("Chave deve ter 44 dígitos");

            Value = value;
        }
    }
}
