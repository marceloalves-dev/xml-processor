using TaxDocumentProcessor.Domain.Entities;

namespace TaxDocumentProcessor.Application.Services
{
    public interface INotaFiscalParser
    {
        public NotaFiscal Parse(string xmlContent);
    }
}
