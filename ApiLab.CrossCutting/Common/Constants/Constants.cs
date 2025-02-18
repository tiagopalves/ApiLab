namespace ApiLab.CrossCutting.Common.Constants
{
    public struct Constants
    {
        public const string API_KEY_HEADER_KEY = "ApiKey";
        public const string CORRELATION_HEADER_KEY = "CorrelationId";
        public const string FLOW_ID_HEADER_KEY = "FlowId";
        public const string REQUEST_ID_PROBLEM_KEY = "requestId";
        public const string TRACE_ID_PROBLEM_KEY = "traceId";
        
        public const string REDIS_CLIENTE_KEY_PREFIX = "Cliente";

        public const string CLIENTES_ENDPOINT = "/clientes/";
    }
}
