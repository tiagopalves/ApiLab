namespace ApiLab.CrossCutting.Configurations
{
    public class AccessConfiguration
    {
        public string ApiTokenSecurityKey { get; set; } = string.Empty;

        public int ApiTokenExpirationTimeInMinutes { get; set; }

        public bool AccessRestriction { get; set; }

        public string AuthorizedApiKeys { get; set; } = string.Empty;
    }
}
