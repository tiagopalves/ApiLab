namespace ApiLab.CrossCutting.Configurations
{
    public class RedisConfiguration
    {
        public string ConnectionString { get; set; } = string.Empty;

        public string HealthCheckName { get; set; } = string.Empty;
    }
}
