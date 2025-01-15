using ApiLab.CrossCutting.Issuer;
using ApiLab.CrossCutting.LogManager.Interfaces;

namespace ApiLab.CrossCutting.LogManager
{
    public class LogManager : ILogManager
    {
        public void AddTrace(string message, string correlationId = "", string flowId = "", object? informationData = null)
        {
            throw new NotImplementedException();
        }
        public void AddInformation(string message, string correlationId = "", string flowId = "", object? informationData = null)
        {
            throw new NotImplementedException();
        }
        public void AddWarning(Issues issue, string message, string correlationId = "", string flowId = "", Exception? ex = null, object? informationData = null)
        {
            throw new NotImplementedException();
        }
        public void AddError(Issues issue, string message, Exception? ex = null, string correlationId = "", string flowId = "", object? informationData = null)
        {
            throw new NotImplementedException();
        }
    }
}
