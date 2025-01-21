using System.Diagnostics.CodeAnalysis;

namespace ApiLab.CrossCutting.Issuer
{
    [ExcludeFromCodeCoverage]
    public class IssuerData
    {
        public string Sigla { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string Prefix => Sigla + "." + ProjectName;
        public string IssuerNumber { get; set; } = string.Empty;
    }
}
