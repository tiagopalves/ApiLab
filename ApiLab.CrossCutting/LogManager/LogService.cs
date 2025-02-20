using ApiLab.CrossCutting.Common.Constants;
using ApiLab.CrossCutting.LogManager.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using Serilog.Events;

namespace ApiLab.CrossCutting.LogManager
{
    public class LogService(ILogger<LogService> logger) : ILogService
    {
        private readonly ILogger<LogService> _logger = logger;

        public void Write(IList<LogInfo> logInfos, string? prefix = "")
        {
            foreach (var logInfo in logInfos)
            {
                Write(logInfo, prefix);
            }
        }

        public void Write(LogInfo logInfo, string? prefix = "")
        {
            var levelId = (int)logInfo.Level;
            var level = (LogEventLevel)levelId;

            var message = logInfo.Message;

            if (!string.IsNullOrEmpty(logInfo.Code))
                message = $"{logInfo.Code} - {message}";
            else if (!string.IsNullOrEmpty(prefix))
                message = $"{prefix} - {message}";

            WriteLog(level, message, logInfo.Exception, logInfo.InformationData, logInfo.CorrelationId, logInfo.FlowId);
        }

        public void LogInformation(string message, object? data = null) =>
             WriteLog(LogEventLevel.Information, message, null, data);

        public void LogWarning(string message, Exception? exception = null, object? data = null) =>
            WriteLog(LogEventLevel.Warning, message, exception, data);

        public void LogError(string message, Exception? exception = null, object? data = null) =>
            WriteLog(LogEventLevel.Error, message, exception, data);

        private void WriteLog(LogEventLevel level, string message, Exception? exception = null, object? data = null, string? correlationId = null, string? flowId = null)
        {
            if (!string.IsNullOrEmpty(correlationId))
                LogContext.PushProperty(Constants.CORRELATION_HEADER_KEY, correlationId);

            if (!string.IsNullOrEmpty(flowId))
                LogContext.PushProperty(Constants.FLOW_ID_HEADER_KEY, flowId);

            var msg = "{@InformationData}";
            var sep = data is not null ? " - " : string.Empty;
            msg = $"{message}{sep}{msg}";

            _logger.Log(logLevel: (LogLevel)level, message: msg, args: data ?? string.Empty, exception: exception);
        }
    }
}