using Tax_Document_Processor.Domain.Entities;

namespace Tax_Document_Processor.Application.Services
{
    public interface INotaFiscalParser
    {
        public NotaFiscal Parse(string xmlContent);
    }
}
