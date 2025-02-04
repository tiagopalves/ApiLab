using ApiLab.CrossCutting.Resources;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiLab.CrossCutting.Common
{
    public class GeneralExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemDetailsService = problemDetailsService;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = exception.Message,
                Detail = exception.StackTrace?.Replace("\n", "").Replace("\t", "").Replace("\r", "").Trim(),
                Type = FriendlyMessages.ProblemDetailsBadRequest
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

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
