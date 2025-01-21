namespace ApiLab.CrossCutting.Issuer.Interfaces
{
    public interface IIssuer
    {
        string? Prefix { get; }
        string MakerCode(Issues issue);
        string MakerProtocol(Issues issue);
    }
}
