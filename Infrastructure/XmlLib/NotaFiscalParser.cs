using Tax_Document_Processor.Application.Services;
using Tax_Document_Processor.Domain.Entities;

namespace Tax_Document_Processor.Infrastructure.XmlLib
{
    public class NotaFiscalParser : INotaFiscalParser
    {
        public NotaFiscal Parse(string xmlContent)
        {
            throw new NotImplementedException();
        }

        //private Nfse ConvertNfse(string xmlContent)
        //{
        //    return new Nfse();
        //}

        //private Nfe ConvertNfe(string xmlContent)
        //{
        //    return new Nfe();
        //}
        //private Nfce ConvertNfce(string xmlContent)
        //{
        //    return new Nfce();
        //}
    }
}
