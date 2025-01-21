namespace ApiLab.CrossCutting.Issuer
{
    public class ProtocoloIssuerBase
    {
        public required IssuerData IssuerData { get; set; }

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
