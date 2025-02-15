namespace ApiLab.CrossCutting.Configurations
{
    public class AccessConfiguration
    {
        public bool AccessRestriction { get; set; }

        public string AuthorizedApiKeys { get; set; } = string.Empty;
    }
}
