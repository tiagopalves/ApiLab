using ApiLab.CrossCutting.Issuer.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace ApiLab.CrossCutting.Issuer
{
    [ExcludeFromCodeCoverage]
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