using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorConf2021.Server.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlazorConf2021.Shared;

namespace BlazorConf2021.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherForecastService service;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,WeatherForecastService service)
        {
            _logger = logger;
            this.service = service;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return service.Get();
        }
    }
}
