using System.Diagnostics.CodeAnalysis;

namespace ApiLab.CrossCutting.Configurations
{
    [ExcludeFromCodeCoverage]
    public class RedisConfiguration
    {
        public string ConnectionString { get; set; } = string.Empty;

        public string HealthCheckName { get; set; } = string.Empty;
    }
}
