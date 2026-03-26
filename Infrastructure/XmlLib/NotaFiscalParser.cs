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
            var ns = "http://www.portalfiscal.inf.br/nfe";

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
            var doc = XDocument.Parse(xmlContent);
            var ns = "http://www.sped.fazenda.gov.br/nfse";

            var infNFSe = doc.Descendants(ns + "infNFSe").First();
            var emit = infNFSe.Element(ns + "emit")!;
            var infDPS = doc.Descendants(ns + "infDPS").First();
            var toma = infDPS.Element(ns + "toma")!;

            var chave = infNFSe.Attribute("Id")!.Value[3..];
            var cnpjEmit = emit.Element(ns + "CNPJ")!.Value;
            var cnpjDest = toma.Element(ns + "CNPJ")?.Value ?? toma.Element(ns + "CPF")!.Value;
            var razaoSocial = emit.Element(ns + "xNome")!.Value;
            var totalValue = infNFSe.Element(ns + "valores")!.Element(ns + "vLiq")!.Value;
            var dtEmission = DateTime.Parse(infDPS.Element(ns + "dhEmi")!.Value);

            return new Nfse(
                cnpjEmit: new CnpjOrCpf(cnpjEmit),
                cnpjDest: new CnpjOrCpf(cnpjDest),
                razaoSocial: razaoSocial,
                chaveNota: new ChaveNota(chave),
                totalValue: totalValue,
                dtEmission: dtEmission);
        }

        private Cte ConvertCte(string xmlContent)
        {
            var doc = XDocument.Parse(xmlContent);
            var ns = "http://www.portalfiscal.inf.br/cte";

            var infCte = doc.Descendants(ns + "infCte").First();
            var emit = infCte.Element(ns + "emit")!;
            var dest = infCte.Element(ns + "dest")!;
            var ide = infCte.Element(ns + "ide")!;

            var chave = doc.Descendants(ns + "chCTe").FirstOrDefault()?.Value
                              ?? infCte.Attribute("Id")!.Value[3..];
            var cnpjEmit = emit.Element(ns + "CNPJ")!.Value;
            var cnpjDest = dest.Element(ns + "CNPJ")?.Value ?? dest.Element(ns + "CPF")!.Value;
            var razaoSocial = emit.Element(ns + "xNome")!.Value;
            var totalValue = infCte.Descendants(ns + "vTPrest").First().Value;
            var dtEmission = DateTime.Parse(ide.Element(ns + "dhEmi")!.Value);

            return new Cte(
                cnpjEmit: new CnpjOrCpf(cnpjEmit),
                cnpjDest: new CnpjOrCpf(cnpjDest),
                razaoSocial: razaoSocial,
                chaveNota: new ChaveNota(chave),
                totalValue: totalValue,
                dtEmission: dtEmission);
        }
    }
}
