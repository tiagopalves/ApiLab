using Apilab.Application.AppServices.Interfaces;
using ApiLab.CrossCutting.LogManager.Interfaces;
using ApiLab.CrossCutting.Resources;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiLab.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokensController(ILogManager logManager, ITokenService tokenService) : ControllerBase
    {
        private readonly ILogManager _logManager = logManager;
        private readonly ITokenService _tokenService = tokenService;

        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public Results<Ok<string>, BadRequest<ProblemDetails>> GetToken()
        {
            try
            {
                var retorno = _tokenService.GenerateToken();

                if (!string.IsNullOrEmpty(retorno))
                    return TypedResults.Ok(retorno);
                else
                    return TypedResults.BadRequest(new ProblemDetails
                    {
                        Title = FriendlyMessages.ErrorTitle,
                        Detail = $"{FriendlyMessages.TokenGenerationError}",
                        Type = FriendlyMessages.ProblemDetailsBadRequest
                    });
            }
            catch (Exception ex)
            {
                _logManager.AddError(CrossCutting.Issuer.Issues.ControllerError_4004, $"{FriendlyMessages.ErrorEndpoint} {nameof(GetToken)}!", ex);

                return TypedResults.BadRequest(new ProblemDetails
                {
                    Title = FriendlyMessages.ErrorTitle,
                    Detail = $"{FriendlyMessages.ErrorEndpoint} {nameof(GetToken)}!",
                    Type = FriendlyMessages.ProblemDetailsBadRequest
                });
            }
        }
    }
}
