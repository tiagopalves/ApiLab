using Apilab.Application.AppServices.Interfaces;
using ApiLab.CrossCutting.LogManager.Interfaces;
using ApiLab.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiLab.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LabController(ILogManager logManager, ILabService labService) : ControllerBase
    {
        private readonly ILogManager _logManager = logManager;
        private readonly ILabService _labService = labService;

        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [HttpGet]
        [ProducesResponseType(typeof(WeatherForecast[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Results<Ok<WeatherForecast[]>, BadRequest> GetWeatherForecast()
        {
            //TODO: Criar camada de validação
            //TODO: Mover implementação para a LabService

            _logManager.AddInformation($"Início do método {nameof(GetWeatherForecast)}");

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
    }
}
