using System.Xml.Linq;
using Tax_Document_Processor.Application.Services;
using Tax_Document_Processor.Domain.Entities;
using Tax_Document_Processor.Domain.ValueObjects;

namespace Tax_Document_Processor.Infrastructure.XmlLib
{
    public class NotaFiscalParser : INotaFiscalParser
    {
        public NotaFiscal Parse(string xmlContent)
        {
            var root = XDocument.Parse(xmlContent).Root!.Name.LocalName;

            return root switch
            {
                "nfeProc" or "NFe" => ConvertNfe(xmlContent),
                "cteProc" or "CTe" => ConvertCte(xmlContent),
                _ => ConvertNfse(xmlContent)
            };
        }

        private Nfe ConvertNfe(string xmlContent)
        {
            var doc = XDocument.Parse(xmlContent);
            XNamespace ns = "http://www.portalfiscal.inf.br/nfe";

            var infNFe = doc.Descendants(ns + "infNFe").First();
            var emit = infNFe.Element(ns + "emit")!;
            var dest = infNFe.Element(ns + "dest")!;
            var ide = infNFe.Element(ns + "ide")!;

            var chave = doc.Descendants(ns + "chNFe").FirstOrDefault()?.Value
                             ?? infNFe.Attribute("Id")!.Value[3..];
            var cnpjEmit = emit.Element(ns + "CNPJ")!.Value;
            var cnpjDest = dest.Element(ns + "CNPJ")?.Value ?? dest.Element(ns + "CPF")!.Value;
            var razaoSocial = emit.Element(ns + "xNome")!.Value;
            var totalValue = infNFe.Descendants(ns + "vNF").First().Value;
            var dtEmission = DateTime.Parse(ide.Element(ns + "dhEmi")!.Value);

            return new Nfe(
                cnpjEmit: new CnpjOrCpf(cnpjEmit),
                cnpjDest: new CnpjOrCpf(cnpjDest),
                razaoSocial: razaoSocial,
                chaveNota: new ChaveNota(chave),
                totalValue: totalValue,
                dtEmission: dtEmission);
        }

        private Nfse ConvertNfse(string xmlContent)
        {
            throw new NotImplementedException();
        }

        private Cte ConvertCte(string xmlContent)
        {
            throw new NotImplementedException();
        }
    }
}
