using ApiLab.CrossCutting.Common.Constants;
using ApiLab.CrossCutting.Issuer;
using ApiLab.CrossCutting.LogManager.Interfaces;
using ApiLab.CrossCutting.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace ApiLab.Api.Common.Filters
{
    public class ApiKeyValidationFilter(ILogManager logManager) : IActionFilter
    {
        private readonly ILogManager _logManager = logManager;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //No Code
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context is not null && !IsAuthorized(context))
                context.Result = new UnauthorizedResult();
        }

        bool IsAuthorized(ActionExecutingContext? context)
        {
            var correlationId = context?.HttpContext?.Request?.Headers[Constants.CORRELATION_HEADER_KEY].ToString() ?? string.Empty;
            var flowId = context?.HttpContext?.Request?.Headers[Constants.FLOW_ID_HEADER_KEY].ToString() ?? string.Empty;

            try
            {
                if (string.IsNullOrEmpty(correlationId))
                {
                    correlationId = Guid.CreateVersion7().ToString();
                    context?.HttpContext.Request.Headers.Append(Constants.CORRELATION_HEADER_KEY, correlationId);
                }

                if (string.IsNullOrEmpty(flowId))
                {
                    flowId = Guid.CreateVersion7().ToString();
                    context?.HttpContext.Request.Headers.Append(Constants.FLOW_ID_HEADER_KEY, flowId);
                }

                //TODO: Implementar validação da chave de API
                return true;
            }
            catch (Exception ex)
            {
                _logManager.AddError(Issues.FilterError_0001, FriendlyMessages.GeneralFail, ex, correlationId, flowId);

                return false;
            }
        }
    }
}
