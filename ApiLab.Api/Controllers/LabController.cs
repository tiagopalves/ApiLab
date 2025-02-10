using Apilab.Application.AppServices.Interfaces;
using ApiLab.CrossCutting.LogManager.Interfaces;
using ApiLab.CrossCutting.Resources;
using ApiLab.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiLab.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LabController(ILogManager logManager, IClienteService clienteService) : ControllerBase
    {
        private readonly ILogManager _logManager = logManager;
        private readonly IClienteService _clienteService = clienteService;

        //TODO: continuar implementaçao da cliente service e padronizar nomes, rotas, etc.

        [HttpPost]
        //[Authorize]
        [ProducesResponseType(typeof(Cliente), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok<Cliente>, BadRequest<ProblemDetails>>> CreateAsync([FromBody]Cliente cliente)
        {
            try
            {
                await _clienteService.CreateAsync(cliente);

                return TypedResults.Ok(cliente);
            }
            catch (Exception ex)
            {
                _logManager.AddError(CrossCutting.Issuer.Issues.ControllerError_4001, $"Erro na chamada do endpoint {nameof(CreateAsync)}!", ex);

                //TODO: Centralizar tratamento de erros
                return TypedResults.BadRequest(new ProblemDetails
                {
                    Title = "Erro!",
                    Detail = $"Erro na chamada do endpoint {nameof(CreateAsync)}!",
                    Type = FriendlyMessages.ProblemDetailsBadRequest
                });
            }
        }

        [HttpGet]
        //[Authorize]
        [ProducesResponseType(typeof(WeatherForecast[]), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok<List<Cliente>>, BadRequest<ProblemDetails>>> GetAllAsync()
        {
            try
            {
                _logManager.AddInformation($"Início do método {nameof(GetAllAsync)}");

                var retorno = await _clienteService.GetAllAsync();

                _logManager.AddInformation($"Fim do método {nameof(GetAllAsync)}");

                return TypedResults.Ok(retorno);
            }
            catch (Exception ex)
            {
                _logManager.AddError(CrossCutting.Issuer.Issues.ControllerError_4002, $"Erro na chamada do endpoint {nameof(GetAllAsync)}!", ex);

                //TODO: Centralizar tratamento de erros
                return TypedResults.BadRequest(new ProblemDetails
                {
                    Title = "Erro!",
                    Detail = $"Erro na chamada do endpoint {nameof(GetAllAsync)}!",
                    Type = FriendlyMessages.ProblemDetailsBadRequest
                });
            }
        }
    }
}
