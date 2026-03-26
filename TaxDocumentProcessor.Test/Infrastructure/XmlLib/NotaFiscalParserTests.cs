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

        // ── CT-e ────────────────────────────────────────────────────────────

        private const string CteNs = "http://www.portalfiscal.inf.br/cte";
        private const string CteChave = "35260305366953000272570030002455531798691955";
        private const string CteCnpjEmit = "05366953000272";
        private const string CteCnpjDest = "02310194000238";
        private const string CteRazaoSocial = "TRANSPORTES MAROSO LTDA";
        private const string CteTotalValue = "121.41";
        private const string CteDhEmi = "2026-03-03T04:55:53-03:00";

        private static string BuildCteProc(string destDoc) => $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<cteProc versao=""4.00"" xmlns=""{CteNs}"">
  <CTe>
    <infCte Id=""CTe{CteChave}"" versao=""4.00"">
      <ide><dhEmi>{CteDhEmi}</dhEmi></ide>
      <emit>
        <CNPJ>{CteCnpjEmit}</CNPJ>
        <xNome>{CteRazaoSocial}</xNome>
      </emit>
      <dest>{destDoc}</dest>
      <vPrest><vTPrest>{CteTotalValue}</vTPrest></vPrest>
    </infCte>
  </CTe>
  <protCTe versao=""4.00"">
    <infProt><chCTe>{CteChave}</chCTe></infProt>
  </protCTe>
</cteProc>";

        private static string BuildCteSemProtocolo() => $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<CTe xmlns=""{CteNs}"">
  <infCte Id=""CTe{CteChave}"" versao=""4.00"">
    <ide><dhEmi>{CteDhEmi}</dhEmi></ide>
    <emit>
      <CNPJ>{CteCnpjEmit}</CNPJ>
      <xNome>{CteRazaoSocial}</xNome>
    </emit>
    <dest><CNPJ>{CteCnpjDest}</CNPJ></dest>
    <vPrest><vTPrest>{CteTotalValue}</vTPrest></vPrest>
  </infCte>
</CTe>";

        [Test]
        public void ShouldReturnCte_WhenRootIsCteProc()
        {
            var xml = BuildCteProc($"<CNPJ>{CteCnpjDest}</CNPJ>");

            var result = _parser.Parse(xml);

            result.Should().BeOfType<Cte>();
        }

        [Test]
        public void ShouldReturnCte_WhenRootIsCTe()
        {
            var xml = BuildCteSemProtocolo();

            var result = _parser.Parse(xml);

            result.Should().BeOfType<Cte>();
        }

        [Test]
        public void Cte_ShouldExtractChave_FromProtCTe()
        {
            var xml = BuildCteProc($"<CNPJ>{CteCnpjDest}</CNPJ>");

            var result = _parser.Parse(xml);

            result.ChaveNota.Value.Should().Be(CteChave);
        }

        [Test]
        public void Cte_ShouldExtractChave_FromIdAttribute_WhenProtCTeAbsent()
        {
            var xml = BuildCteSemProtocolo();

            var result = _parser.Parse(xml);

            result.ChaveNota.Value.Should().Be(CteChave);
        }

        [Test]
        public void Cte_ShouldExtractCnpjEmit()
        {
            var xml = BuildCteProc($"<CNPJ>{CteCnpjDest}</CNPJ>");

            var result = _parser.Parse(xml);

            result.CnpjEmit.Value.Should().Be(CteCnpjEmit);
        }

        [Test]
        public void Cte_ShouldExtractCnpjDest()
        {
            var xml = BuildCteProc($"<CNPJ>{CteCnpjDest}</CNPJ>");

            var result = _parser.Parse(xml);

            result.CnpjDest.Value.Should().Be(CteCnpjDest);
            result.CnpjDest.IsCpf.Should().BeFalse();
        }

        [Test]
        public void Cte_ShouldExtractRazaoSocial()
        {
            var xml = BuildCteProc($"<CNPJ>{CteCnpjDest}</CNPJ>");

            var result = _parser.Parse(xml);

            result.RazaoSocial.Should().Be(CteRazaoSocial);
        }

        [Test]
        public void Cte_ShouldExtractTotalValue()
        {
            var xml = BuildCteProc($"<CNPJ>{CteCnpjDest}</CNPJ>");

            var result = _parser.Parse(xml);

            result.TotalValue.Should().Be(CteTotalValue);
        }

        [Test]
        public void Cte_ShouldExtractDtEmission()
        {
            var xml = BuildCteProc($"<CNPJ>{CteCnpjDest}</CNPJ>");

            var result = _parser.Parse(xml);

            result.DtEmission.Should().Be(DateTime.Parse(CteDhEmi));
        }

        // ── NFS-e ───────────────────────────────────────────────────────────

        private const string NfseNs = "http://www.sped.fazenda.gov.br/nfse";
        private const string NfseChave = "33045572261646079000117000000000001326036395556327"; // 50 dígitos
        private const string NfseCnpjEmit = "61646079000117";
        private const string NfseCnpjDest = "07792269000105";
        private const string NfseRazaoSocial = "ITS SERVICOS DE TECNOLOGIA LTDA";
        private const string NfseTotalValue = "400.00";
        private const string NfseDhEmi = "2026-03-04T12:06:56-03:00";

        private static string BuildNfse(string tomaDoc) => $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<NFSe versao=""1.01"" xmlns=""{NfseNs}"">
  <infNFSe Id=""NFS{NfseChave}"">
    <emit>
      <CNPJ>{NfseCnpjEmit}</CNPJ>
      <xNome>{NfseRazaoSocial}</xNome>
    </emit>
    <valores><vLiq>{NfseTotalValue}</vLiq></valores>
    <DPS versao=""1.01"" xmlns=""{NfseNs}"">
      <infDPS Id=""DPS123"">
        <dhEmi>{NfseDhEmi}</dhEmi>
        <toma>{tomaDoc}</toma>
      </infDPS>
    </DPS>
  </infNFSe>
</NFSe>";

        [Test]
        public void ShouldReturnNfse_WhenRootIsNFSe()
        {
            var xml = BuildNfse($"<CNPJ>{NfseCnpjDest}</CNPJ>");

            var result = _parser.Parse(xml);

            result.Should().BeOfType<Nfse>();
        }

        [Test]
        public void Nfse_ShouldExtractChave_FromIdAttribute()
        {
            var xml = BuildNfse($"<CNPJ>{NfseCnpjDest}</CNPJ>");

            var result = _parser.Parse(xml);

            result.ChaveNota.Value.Should().Be(NfseChave);
            result.ChaveNota.IsNfse.Should().BeTrue();
        }

        [Test]
        public void Nfse_ShouldExtractCnpjEmit()
        {
            var xml = BuildNfse($"<CNPJ>{NfseCnpjDest}</CNPJ>");

            var result = _parser.Parse(xml);

            result.CnpjEmit.Value.Should().Be(NfseCnpjEmit);
        }

        [Test]
        public void Nfse_ShouldExtractCnpjDest_WhenTomaIsCnpj()
        {
            var xml = BuildNfse($"<CNPJ>{NfseCnpjDest}</CNPJ>");

            var result = _parser.Parse(xml);

            result.CnpjDest.Value.Should().Be(NfseCnpjDest);
            result.CnpjDest.IsCpf.Should().BeFalse();
        }

        [Test]
        public void Nfse_ShouldExtractCpfDest_WhenTomaIsCpf()
        {
            var xml = BuildNfse($"<CPF>30575868805</CPF>");

            var result = _parser.Parse(xml);

            result.CnpjDest.Value.Should().Be("30575868805");
            result.CnpjDest.IsCpf.Should().BeTrue();
        }

        [Test]
        public void Nfse_ShouldExtractRazaoSocial()
        {
            var xml = BuildNfse($"<CNPJ>{NfseCnpjDest}</CNPJ>");

            var result = _parser.Parse(xml);

            result.RazaoSocial.Should().Be(NfseRazaoSocial);
        }

        [Test]
        public void Nfse_ShouldExtractTotalValue()
        {
            var xml = BuildNfse($"<CNPJ>{NfseCnpjDest}</CNPJ>");

            var result = _parser.Parse(xml);

            result.TotalValue.Should().Be(NfseTotalValue);
        }

        [Test]
        public void Nfse_ShouldExtractDtEmission()
        {
            var xml = BuildNfse($"<CNPJ>{NfseCnpjDest}</CNPJ>");

            var result = _parser.Parse(xml);

            result.DtEmission.Should().Be(DateTime.Parse(NfseDhEmi));
        }
    }
}
