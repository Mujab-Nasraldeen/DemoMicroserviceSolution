using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Service1WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly Random _random;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _random = new Random();
        }

        //[Authorize(Roles = "Administrator")]
        [HttpGet(Name = "GetWeatherForecast")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetWeatherForecast()
        {
            try
            {
                // Generate forecast data
                var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = _random.Next(-20, 55),
                    Summary = Summaries[_random.Next(Summaries.Length)]
                }).ToArray();

                // Log response for debugging
                _logger.LogInformation("Weather forecast successfully generated.");

                // Add metadata for response debugging
                Response.Headers.Add("X-Generated-On", DateTime.UtcNow.ToString("o"));

                return Ok(forecasts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating weather forecast.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
