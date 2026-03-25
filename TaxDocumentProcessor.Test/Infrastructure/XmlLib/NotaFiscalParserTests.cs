using FluentAssertions;
using Tax_Document_Processor.Domain.Entities;
using Tax_Document_Processor.Infrastructure.XmlLib;

namespace Tax_Document_Processor.Tests.Infrastructure.XmlLib
{
    [TestFixture]
    public class NotaFiscalParserTests
    {
        private NotaFiscalParser _parser;

        private const string Ns = "http://www.portalfiscal.inf.br/nfe";
        private const string Chave = "35390114002097000186550010000246821000246820";
        private const string CnpjEmit = "14002097000186";
        private const string CnpjDestValue = "11222333000181";
        private const string CpfDestValue = "30575868805";
        private const string RazaoSocial = "EMPRESA TESTE LTDA";
        private const string TotalValue = "46.69";
        private const string DhEmi = "2039-01-16T00:00:00-02:00";

        [SetUp]
        public void SetUp()
        {
            _parser = new NotaFiscalParser();
        }

        private static string BuildNfeProc(string destDoc) => $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<nfeProc versao=""4.00"" xmlns=""{Ns}"">
  <NFe>
    <infNFe versao=""4.00"" Id=""NFe{Chave}"">
      <ide><dhEmi>{DhEmi}</dhEmi></ide>
      <emit>
        <CNPJ>{CnpjEmit}</CNPJ>
        <xNome>{RazaoSocial}</xNome>
      </emit>
      <dest>{destDoc}</dest>
      <total><ICMSTot><vNF>{TotalValue}</vNF></ICMSTot></total>
    </infNFe>
  </NFe>
  <protNFe versao=""4.00"">
    <infProt><chNFe>{Chave}</chNFe></infProt>
  </protNFe>
</nfeProc>";

        private static string BuildNfeSemProtocolo(string destDoc) => $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<NFe xmlns=""{Ns}"">
  <infNFe versao=""4.00"" Id=""NFe{Chave}"">
    <ide><dhEmi>{DhEmi}</dhEmi></ide>
    <emit>
      <CNPJ>{CnpjEmit}</CNPJ>
      <xNome>{RazaoSocial}</xNome>
    </emit>
    <dest>{destDoc}</dest>
    <total><ICMSTot><vNF>{TotalValue}</vNF></ICMSTot></total>
  </infNFe>
</NFe>";

        [Test]
        public void ShouldReturnNfe_WhenRootIsNfeProc()
        {
            var xml = BuildNfeProc($"<CNPJ>{CnpjDestValue}</CNPJ>");

            var result = _parser.Parse(xml);

            result.Should().BeOfType<Nfe>();
        }

        [Test]
        public void ShouldReturnNfe_WhenRootIsNfe()
        {
            var xml = BuildNfeSemProtocolo($"<CNPJ>{CnpjDestValue}</CNPJ>");

            var result = _parser.Parse(xml);

            result.Should().BeOfType<Nfe>();
        }

        [Test]
        public void ShouldExtractChave_FromProtNFe()
        {
            var xml = BuildNfeProc($"<CNPJ>{CnpjDestValue}</CNPJ>");

            var result = _parser.Parse(xml);

            result.ChaveNota.Value.Should().Be(Chave);
        }

        [Test]
        public void ShouldExtractChave_FromIdAttribute_WhenProtNFeAbsent()
        {
            var xml = BuildNfeSemProtocolo($"<CNPJ>{CnpjDestValue}</CNPJ>");

            var result = _parser.Parse(xml);

            result.ChaveNota.Value.Should().Be(Chave);
        }

        [Test]
        public void ShouldExtractCnpjEmit()
        {
            var xml = BuildNfeProc($"<CNPJ>{CnpjDestValue}</CNPJ>");

            var result = _parser.Parse(xml);

            result.CnpjEmit.Value.Should().Be(CnpjEmit);
        }

        [Test]
        public void ShouldExtractCnpjDest_WhenDestIsCnpj()
        {
            var xml = BuildNfeProc($"<CNPJ>{CnpjDestValue}</CNPJ>");

            var result = _parser.Parse(xml);

            result.CnpjDest.Value.Should().Be(CnpjDestValue);
            result.CnpjDest.IsCpf.Should().BeFalse();
        }

        [Test]
        public void ShouldExtractCpfDest_WhenDestIsCpf()
        {
            var xml = BuildNfeProc($"<CPF>{CpfDestValue}</CPF>");

            var result = _parser.Parse(xml);

            result.CnpjDest.Value.Should().Be(CpfDestValue);
            result.CnpjDest.IsCpf.Should().BeTrue();
        }

        [Test]
        public void ShouldExtractRazaoSocial()
        {
            var xml = BuildNfeProc($"<CNPJ>{CnpjDestValue}</CNPJ>");

            var result = _parser.Parse(xml);

            result.RazaoSocial.Should().Be(RazaoSocial);
        }

        [Test]
        public void ShouldExtractTotalValue()
        {
            var xml = BuildNfeProc($"<CNPJ>{CnpjDestValue}</CNPJ>");

            var result = _parser.Parse(xml);

            result.TotalValue.Should().Be(TotalValue);
        }

        [Test]
        public void ShouldExtractDtEmission()
        {
            var xml = BuildNfeProc($"<CNPJ>{CnpjDestValue}</CNPJ>");

            var result = _parser.Parse(xml);

            result.DtEmission.Should().Be(DateTime.Parse(DhEmi));
        }
    }
}
