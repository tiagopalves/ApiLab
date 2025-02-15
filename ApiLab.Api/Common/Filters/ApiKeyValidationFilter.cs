using ApiLab.CrossCutting.Common.Constants;
using ApiLab.CrossCutting.Configurations;
using ApiLab.CrossCutting.Issuer;
using ApiLab.CrossCutting.LogManager.Interfaces;
using ApiLab.CrossCutting.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace ApiLab.Api.Common.Filters
{
    public class ApiKeyValidationFilter(ILogManager logManager, IOptionsMonitor<AccessConfiguration> accessConfiguration) : IActionFilter
    {
        private readonly ILogManager _logManager = logManager;
        private readonly IOptionsMonitor<AccessConfiguration> _accessConfiguration = accessConfiguration;

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
            var apiKey = context?.HttpContext?.Request?.Headers[Constants.API_KEY_HEADER_KEY].ToString() ?? string.Empty;
            var informationData = new { apiKey };
            var isAuthorized = false;

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

                if (_accessConfiguration.CurrentValue.AccessRestriction)
                {
                    if (string.IsNullOrEmpty(apiKey))
                        _logManager.AddError(Issues.FilterError_0001, FriendlyMessages.ApiKeyValidationNotFound, correlationId: correlationId, flowId: flowId, informationData: informationData);
                    else if (!_accessConfiguration.CurrentValue.AuthorizedApiKeys.Contains(apiKey))
                        _logManager.AddError(Issues.FilterError_0002, FriendlyMessages.ApiKeyValidationNotAuthorized, correlationId: correlationId, flowId: flowId, informationData: informationData);
                    else
                    {
                        _logManager.AddTrace(FriendlyMessages.ApiKeyValidationAuthorized, correlationId: correlationId, flowId: flowId, informationData: informationData);
                        isAuthorized = true;
                    }
                }
                else
                    isAuthorized = true;
            }
            catch (Exception ex)
            {
                _logManager.AddError(Issues.FilterError_0003, FriendlyMessages.ApiKeyValidationError, ex, correlationId, flowId, informationData: informationData);
            }

            return isAuthorized;
        }
    }
}
