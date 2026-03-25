namespace Tax_Document_Processor.Domain.ValueObjects
{
    public class CnpjOrCpf
    {
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

        private static string Clean(string value) =>
            new string(value.Where(char.IsDigit).ToArray());

        private static bool IsValidCpf(string cpf)
        {
            if (cpf.Distinct().Count() == 1)
                return false;

            return ValidateCpfDigit(cpf, 9) && ValidateCpfDigit(cpf, 10);
        }

        private static bool ValidateCpfDigit(string cpf, int position)
        {
            var sum = cpf
                .Take(position)
                .Select((c, i) => (c - '0') * (position + 1 - i))
                .Sum();

            var remainder = (sum * 10) % 11;
            var expectedDigit = remainder == 10 ? 0 : remainder;

            return (cpf[position] - '0') == expectedDigit;
        }

        private static bool IsValidCnpj(string cnpj)
        {
            if (cnpj.Distinct().Count() == 1)
                return false;

            return ValidateCnpjDigit(cnpj, 12) && ValidateCnpjDigit(cnpj, 13);
        }

        private static bool ValidateCnpjDigit(string cnpj, int position)
        {
            int[] multipliers = position == 12
                ? new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 }
                : new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            var sum = cnpj
                .Take(position)
                .Select((c, i) => (c - '0') * multipliers[i])
                .Sum();

            var remainder = sum % 11;
            var expectedDigit = remainder < 2 ? 0 : 11 - remainder;

            return (cnpj[position] - '0') == expectedDigit;
        }
    }
}
