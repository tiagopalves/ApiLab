namespace ApiLab.CrossCutting.LogManager.Interfaces
{
    public interface ILogService
    {
        void Write(IList<LogInfo> logInfos, string? prefix = "");

        void Write(LogInfo logInfo, string? prefix = "");

        void LogInfo(string message, object? data = null);

        void LogWarning(string message, Exception? exception = null, object? data = null);

        void LogError(string message, Exception? exception = null, object? data = null);
    }

}