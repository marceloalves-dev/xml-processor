namespace TaxDocumentProcessor.Domain.ValueObjects
{
    public class CnpjOrCpf
    {
        private static readonly int[] CnpjWeights1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        private static readonly int[] CnpjWeights2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        public string Value { get; private set; }
        public bool IsCpf { get; private set; }

        public CnpjOrCpf(string value)
        {
            var cleaned = Clean(value);

            if (cleaned.Length == 11 && IsValidCpf(cleaned))
            {
                IsCpf = true;
                Value = cleaned;
            }
            else if (cleaned.Length == 14 && IsValidCnpj(cleaned))
            {
                IsCpf = false;
                Value = cleaned;
            }
            else
            {
                throw new ArgumentException("CNPJ ou CPF inválido.");
            }
        }

        private static string Clean(string value)
        {
            var buffer = new char[value.Length];
            var count = 0;
            foreach (var c in value)
            {
                if (char.IsDigit(c))
                    buffer[count++] = c;
            }
            return new string(buffer, 0, count);
        }

        private static bool IsValidCpf(string cpf)
        {
            var first = cpf[0];
            for (var i = 1; i < 11; i++)
            {
                if (cpf[i] != first) break;
                if (i == 10) return false;
            }

            return ValidateCpfDigit(cpf, 9) && ValidateCpfDigit(cpf, 10);
        }

        private static bool ValidateCpfDigit(string cpf, int position)
        {
            var sum = 0;
            for (var i = 0; i < position; i++)
                sum += (cpf[i] - '0') * (position + 1 - i);

            var remainder = (sum * 10) % 11;
            var expectedDigit = remainder == 10 ? 0 : remainder;

            return (cpf[position] - '0') == expectedDigit;
        }

        private static bool IsValidCnpj(string cnpj)
        {
            var first = cnpj[0];
            for (var i = 1; i < 14; i++)
            {
                if (cnpj[i] != first) break;
                if (i == 13) return false;
            }

            return ValidateCnpjDigit(cnpj, 12) && ValidateCnpjDigit(cnpj, 13);
        }

        private static bool ValidateCnpjDigit(string cnpj, int position)
        {
            var weights = position == 12 ? CnpjWeights1 : CnpjWeights2;

            var sum = 0;
            for (var i = 0; i < position; i++)
                sum += (cnpj[i] - '0') * weights[i];

            var remainder = sum % 11;
            var expectedDigit = remainder < 2 ? 0 : 11 - remainder;

            return (cnpj[position] - '0') == expectedDigit;
        }
    }
}
