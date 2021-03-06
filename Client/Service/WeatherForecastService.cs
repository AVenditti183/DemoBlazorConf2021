using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorConf2021.Shared;

namespace BlazorConf2021.Client.Service
{
    public interface IWeatherForecastService
    {
        Task<List<WeatherForecast>> Get();
    }

    public class WeatherForecastService : IWeatherForecastService
    {
        HttpClient Http;

        public WeatherForecastService(HttpClient Http)
        {
            this.Http = Http;
        }

        public Task<List<WeatherForecast>> Get()
            => Http.GetFromJsonAsync<List<WeatherForecast>>("WeatherForecast");
    }
}
