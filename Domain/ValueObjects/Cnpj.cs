namespace Tax_Document_Processor.Domain.ValueObjects
{
    public class Cnpj
    {
        public string Value { get; private set; }

        public Cnpj(string value)
        {
            var cleaned = Clean(value);

            if (!IsValid(cleaned))
                throw new ArgumentException("CNPJ inválido.");

            Value = cleaned;
        }

        private static string Clean(string value) =>
            new string(value.Where(char.IsDigit).ToArray());

        private static bool IsValid(string cnpj)
        {
            if (cnpj.Length != 14)
                return false;

            if (cnpj.Distinct().Count() == 1)
                return false;

            return ValidateDigit(cnpj, 12) && ValidateDigit(cnpj, 13);
        }

        private static bool ValidateDigit(string cnpj, int position)
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
