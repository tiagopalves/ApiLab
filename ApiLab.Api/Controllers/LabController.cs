using ApiLab.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApiLab.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LabController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        private readonly ILogger<LabController> _logger;

        public LabController(ILogger<LabController> logger) => _logger = logger;

        [HttpGet]
        public IEnumerable<WeatherForecast> GetWeatherForecast()
        {
            _logger.LogInformation($"Início do método {nameof(GetWeatherForecast)}");

            var retorno = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
            
            _logger.LogInformation($"Fim do método {nameof(GetWeatherForecast)}");

            return retorno;
        }
    }
}
