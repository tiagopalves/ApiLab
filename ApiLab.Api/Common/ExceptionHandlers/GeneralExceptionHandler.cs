using ApiLab.CrossCutting.Issuer;
using ApiLab.CrossCutting.LogManager.Interfaces;
using ApiLab.CrossCutting.Resources;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ApiLab.Api.Common.ExceptionHandlers
{
    public class GeneralExceptionHandler(IProblemDetailsService problemDetailsService, ILogManager logManager) : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemDetailsService = problemDetailsService;
        private readonly ILogManager _logManager = logManager;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logManager.AddError(Issues.GeneralError_500, FriendlyMessages.GeneralFail, exception);

            var problemDetails = new ProblemDetails
            {
                Status = httpContext.Response.StatusCode,
                Title = FriendlyMessages.GeneralFail,
                Detail = exception.Message,
                Type = FriendlyMessages.ProblemDetailsBadRequest
            };

            return await _problemDetailsService.TryWriteAsync(
                new ProblemDetailsContext
                {
                    HttpContext = httpContext,
                    ProblemDetails = problemDetails,
                    Exception = exception
                });
        }
    }
}
