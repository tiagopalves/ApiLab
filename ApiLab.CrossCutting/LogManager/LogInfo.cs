
namespace ApiLab.CrossCutting.LogManager
{
    public class LogInfo
    {
        public string CorrelationId { get; set; } = string.Empty;
        public string FlowId { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public object? InformationData { get; set; }
        public Exception? Exception { get; set; }
        public LoggingLevel Level { get; set; }

        public LogInfo()
        {
            Level = LoggingLevel.Information;
        }
    }
}