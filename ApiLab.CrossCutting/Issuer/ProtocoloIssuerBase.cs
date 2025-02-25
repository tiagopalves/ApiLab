using System.Diagnostics.CodeAnalysis;

namespace ApiLab.CrossCutting.Issuer
{
    [ExcludeFromCodeCoverage]
    public class ProtocoloIssuerBase
    {
        public IssuerData IssuerData { get; set; } = new IssuerData();

        public string? Prefix => IssuerData?.Prefix;
        
        public virtual string MakerCode(string issue)
        {
            return ExtractCode(issue);
        }
        
        public virtual string MakerProtocol(string issue)
        {
            IssuerData.IssuerNumber = ExtractCode(issue);
            return IssuerData.Sigla + "." + IssuerData.ProjectName + "." + IssuerData.IssuerNumber;
        }
        
        private static string ExtractCode(string issue)
        {
            return issue.Split('_').Last();
        }
    }
}
