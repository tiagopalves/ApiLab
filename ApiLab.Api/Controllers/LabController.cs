using Apilab.Application.AppServices.Interfaces;
using Apilab.Application.Validators;
using ApiLab.CrossCutting.LogManager.Interfaces;
using ApiLab.CrossCutting.Resources;
using ApiLab.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiLab.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LabController(ILogManager logManager, ILabService labService, IValidator<Cliente> _clienteValidator) : ControllerBase
    {
        private readonly ILogManager _logManager = logManager;
        private readonly ILabService _labService = labService;
        private readonly IValidator<Cliente> _clienteValidator = _clienteValidator;

        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(WeatherForecast[]), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public Results<Ok<WeatherForecast[]>, BadRequest<ProblemDetails>> GetWeatherForecast()
        {
            //TODO: Mover implementação para a LabService e alterar entrada do método para receber um cliente

            try
            {
                _logManager.AddInformation($"Início do método {nameof(GetWeatherForecast)}");

                var validationResult = _clienteValidator.Validate(new Cliente() { Id = Guid.CreateVersion7(), Nome = "", Email = "" });
                if (!validationResult.IsValid)
                {
                    _logManager.AddInformation(string.Join(" ", validationResult.Errors));
                }
                
                var retorno = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();

                _logManager.AddInformation($"Fim do método {nameof(GetWeatherForecast)}");

                return TypedResults.Ok(retorno);
            }
            catch (Exception ex)
            {
                _logManager.AddError(CrossCutting.Issuer.Issues.ControllerError_4001, $"Erro na chamada do endpoint {nameof(GetWeatherForecast)}!", ex);

                //TODO: Centralizar tratamento de erros
                return TypedResults.BadRequest(new ProblemDetails
                {
                    Title = "Erro!",
                    Detail = $"Erro na chamada do endpoint {nameof(GetWeatherForecast)}!",
                    Type = FriendlyMessages.ProblemDetailsBadRequest
                });
            }
        }
    }
}
