namespace ApiLab.CrossCutting.Configurations
{
    public class HealthChecksConfiguration
    {
        public int EvaluationTimeInSeconds { get; set; }

        public int MaximumHistoryEntriesPerEndpoint { get; set; }

        public string HealthCheckEndpointUri { get; set; } = string.Empty;
    }
}
