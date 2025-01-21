using ApiLab.CrossCutting.Issuer.Interfaces;

namespace ApiLab.CrossCutting.Issuer
{
    public class Issuer : ProtocoloIssuerBase, IIssuer
    {
        public Issuer()
        {
            IssuerData = new IssuerData { Sigla = "XX0", ProjectName = "ApiLab" };
        }

        public string MakerCode(Issues issue)
        {
            return MakerCode(issue.ToString());
        }

        public string MakerProtocol(Issues issue)
        {
            return MakerProtocol(issue.ToString());
        }
    }
}