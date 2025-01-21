using ApiLab.CrossCutting.LogManager.Interfaces;

namespace ApiLab.CrossCutting.LogManager
{
    public class LogService : ILogService
    {
        public void Write(IList<LogInfo> logInfos, string? prefix = "")
        {
            throw new NotImplementedException();
        }

        public void Write(LogInfo logInfo, string? prefix = "")
        {
            throw new NotImplementedException();
        }

        public void LogInfo(string message, object? data = null)
        {
            throw new NotImplementedException();
        }

        public void LogWarning(string message, Exception? exception = null, object? data = null)
        {
            throw new NotImplementedException();
        }
        public void LogError(string message, Exception exception, object? data = null)
        {
            throw new NotImplementedException();
        }

    }
}