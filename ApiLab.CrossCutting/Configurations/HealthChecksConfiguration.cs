using System.Diagnostics.CodeAnalysis;

namespace ApiLab.CrossCutting.Configurations
{
    [ExcludeFromCodeCoverage]
    public class HealthChecksConfiguration
    {
        public int EvaluationTimeInSeconds { get; set; }

        public int MaximumHistoryEntriesPerEndpoint { get; set; }

        public string HealthCheckEndpointUri { get; set; } = string.Empty;
    }
}
