using ApiLab.CrossCutting.Issuer;
using ApiLab.CrossCutting.Issuer.Interfaces;
using ApiLab.CrossCutting.LogManager.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace ApiLab.CrossCutting.LogManager
{
    public class LogManager : ILogManager
    {
        private readonly IIssuer _issuer;
        private readonly ILogService _logService;

        public LogManager(IIssuer issuer,
                          ILogService logService)
        {
            _issuer = issuer;
            _logService = logService;
        }

        public void AddTrace(string message, string correlationId = "", string flowId = "", object? informationData = null)
        {
            LogServiceWrite(LoggingLevel.Trace, message, correlationId, flowId, informationData);
        }
        public void AddInformation(string message, string correlationId = "", string flowId = "", object? informationData = null)
        {
            LogServiceWrite(LoggingLevel.Information, message, correlationId, flowId, informationData);
        }
        public void AddWarning(Issues issue, string message, string correlationId = "", string flowId = "", Exception? ex = null, object? informationData = null)
        {
            LogServiceWrite(LoggingLevel.Warning, message, correlationId, flowId, informationData, issue, ex);
        }
        public void AddError(Issues issue, string message, Exception? ex = null, string correlationId = "", string flowId = "", object? informationData = null)
        {
            LogServiceWrite(LoggingLevel.Error, message, correlationId, flowId, informationData, issue, ex);

        }

        [ExcludeFromCodeCoverage]
        private void LogServiceWrite(LoggingLevel level, string message, string correlationId = "", string flowId = "", object? informationData = null, Issues issue = Issues.None, Exception? ex = null)
        {
            try
            {
                _logService.Write(new LogInfo
                {
                    CorrelationId = correlationId,
                    FlowId = flowId,
                    Level = level,
                    Code = _issuer.MakerProtocol(issue),
                    Message = message,
                    Exception = ex,
                    InformationData = informationData
                }, _issuer.Prefix);
            }
            catch (Exception e)
            {
                _logService.Write(new LogInfo
                {
                    CorrelationId = correlationId,
                    FlowId = flowId,
                    Level = LoggingLevel.Error,
                    Code = _issuer.MakerProtocol(Issues.LogManagerError_5001),
                    Message = _issuer.MakerProtocol(issue) + " - " + message,
                    Exception = e,
                }, _issuer.Prefix);
            }
        }

    }
}
