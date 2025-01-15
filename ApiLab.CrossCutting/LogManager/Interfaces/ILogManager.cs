using ApiLab.CrossCutting.Issuer;

namespace ApiLab.CrossCutting.LogManager.Interfaces
{
    /// <summary> 
    /// Classe responsável pela centralização de gravação de Logs da Aplicação. 
    /// Optamos por passar informações como correlationId e flowId como parâmetros, para obter ganhos de performance na execução com alta concorrência e mantendo a injeção como Singleton.
    public interface ILogManager
    {
        void AddTrace(string message, string correlationId = "", string flowId = "", object? informationData = null); 
        void AddInformation(string message, string correlationId = "", string flowId = "", object? informationData = null); 
        void AddWarning(Issues issue, string message, string correlationId = "", string flowId = "", Exception? ex = null, object? informationData = null); 
        void AddError(Issues issue, string message, Exception? ex = null, string correlationId = "", string flowId = "", object? informationData = null); 
    }
}
