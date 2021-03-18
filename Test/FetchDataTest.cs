using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorConf2021.Client.Pages;
using BlazorConf2021.Client.Service;
using BlazorConf2021.Shared;
using Bunit;
using Bunit.JSInterop;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Test
{
    public class FetchDataTest
    {
        [Fact]
        public void DataIsnull()
        {
            var service = new Mock<IWeatherForecastService>();
            using var ctx = new TestContext();

            ctx.Services.AddSingleton(service.Object);

            service.Setup(o => o.Get())
                .ReturnsAsync(() => null);
            var component = ctx.RenderComponent<FetchData>();

            component.MarkupMatches(@"<h1>Weather forecast</h1>
<p>This component demonstrates fetching data from the server.</p>
<p><em>Loading...</em></p>");
        }

        [Fact]
        public void DataSomeData()
        {
            // Arrange
            var service = new Mock<IWeatherForecastService>();
            using var ctx = new TestContext();
            var firstItem = new WeatherForecast
            {
                Date = DateTime.Now,
                Summary = "Freezing",
                TemperatureC = 20
            };
            var secondItem = new WeatherForecast
            {
                Date = DateTime.Now.AddDays(-2),
                Summary = "Cool",
                TemperatureC = 23
            };

            var weatherForecasts = new List<WeatherForecast>
            { firstItem, secondItem};

            ctx.Services.AddSingleton(service.Object);

            service.Setup(o => o.Get())
                .ReturnsAsync(weatherForecasts);

            var component = ctx.RenderComponent<FetchData>();

            var expectedDoom = @"<h1>Weather forecast</h1>
<p>This component demonstrates fetching data from the server.</p>
    <table class=""table"">
                    <thead>
                <tr>
                <th>Date</th>
                <th>Temp. (C)</th>
                <th>Temp. (F)</th>
                <th>Summary</th>
                <th></th>
                </tr>
                </thead>
                <tbody>";
            foreach (var item in weatherForecasts)
            {
                expectedDoom += $@"<tr>
                    <td>{item.Date.ToShortDateString()}</td>
                    <td>{item.TemperatureC}</td>
                    <td>{item.TemperatureF}</td>
                    <td>{item.Summary}</td>
                    <td><button class=""btn btn-primary"" >Edit</button></td>
                </tr>";
            }

            expectedDoom += @"</tbody></table>";
            component.MarkupMatches(expectedDoom);
        }

        [Fact]
        public void OpenEditItem()
        {
            // Arrange
            var service = new Mock<IWeatherForecastService>();
            using var ctx = new TestContext();
            var firstItem = new WeatherForecast
            {
                Date = DateTime.Now,
                Summary = "Freezing",
                TemperatureC = 20
            };
            var secondItem = new WeatherForecast
            {
                Date = DateTime.Now.AddDays(-2),
                Summary = "Cool",
                TemperatureC = 23
            };

            var weatherForecasts = new List<WeatherForecast>
                { firstItem, secondItem};

            ctx.Services.AddSingleton(service.Object);

            service.Setup(o => o.Get())
                .ReturnsAsync(weatherForecasts);

            var component = ctx.RenderComponent<FetchData>();

            var firstEditButton = component.Find("button");

            firstEditButton.Click();

            var expectedDoom = $@"<h1>Weather forecast</h1>
<p>This component demonstrates fetching data from the server.</p>
<form>
            <div class=""form-group""><label>Date</label>
                <input type=""date"" class=""valid"" value=""{firstItem.Date.ToString("yyyy-MM-dd")}"">
                </div>
                <div class=""form-group""><label>Temp. (C)</label>
                <input step=""any"" type=""number"" class=""valid"" value=""{firstItem.TemperatureC}"">
                </div>
                <div class=""form-group""><label>Summary</label>
                <input class=""valid"" value=""{firstItem.Summary}"">
                </div>
                <button type=""button"">Go Back</button></form>";

            component.MarkupMatches(expectedDoom);
        }

        [Fact]
        public void CloseEditWithoutModified()
        {
            var service = new Mock<IWeatherForecastService>();
            using var ctx = new TestContext();
            var firstItem = new WeatherForecast
            {
                Date = DateTime.Now,
                Summary = "Freezing",
                TemperatureC = 20
            };
            var secondItem = new WeatherForecast
            {
                Date = DateTime.Now.AddDays(-2),
                Summary = "Cool",
                TemperatureC = 23
            };

            var weatherForecasts = new List<WeatherForecast>
                { firstItem, secondItem};

            ctx.Services.AddSingleton(service.Object);
            ctx.JSInterop.SetupVoid("alert", "modifica effettuata").SetVoidResult();
            
            service.Setup(o => o.Get())
                .ReturnsAsync(weatherForecasts);

            var component = ctx.RenderComponent<FetchData>();

            var firstEditButton = component.Find("button");

            firstEditButton.Click();

            var goBackButton = component.Find("button");

            goBackButton.Click();

            var expectedDoom = @"<h1>Weather forecast</h1>
<p>This component demonstrates fetching data from the server.</p>
    <table class=""table"">
                    <thead>
                <tr>
                <th>Date</th>
                <th>Temp. (C)</th>
                <th>Temp. (F)</th>
                <th>Summary</th>
                <th></th>
                </tr>
                </thead>
                <tbody>";
            foreach (var item in weatherForecasts)
            {
                expectedDoom += $@"<tr>
                    <td>{item.Date.ToShortDateString()}</td>
                    <td>{item.TemperatureC}</td>
                    <td>{item.TemperatureF}</td>
                    <td>{item.Summary}</td>
                    <td><button class=""btn btn-primary"" >Edit</button></td>
                </tr>";
            }

            expectedDoom += @"</tbody></table>";

            component.MarkupMatches(expectedDoom);
            ctx.JSInterop.VerifyInvoke("alert", "modifica effettuata");
        }

        [Fact]
        public void FormValidationDate()
        {
            // Arrange
            var service = new Mock<IWeatherForecastService>();
            using var ctx = new TestContext();
            var firstItem = new WeatherForecast
            {
                Date = DateTime.Now,
                Summary = "Freezing",
                TemperatureC = 20
            };
            var secondItem = new WeatherForecast
            {
                Date = DateTime.Now.AddDays(-2),
                Summary = "Cool",
                TemperatureC = 23
            };

            var weatherForecasts = new List<WeatherForecast>
                { firstItem, secondItem};

            ctx.Services.AddSingleton(service.Object);

            service.Setup(o => o.Get())
                .ReturnsAsync(weatherForecasts);

            var component = ctx.RenderComponent<FetchData>();

            var firstEditButton = component.Find("button");

            firstEditButton.Click();

            var inputDate = component.Find("input[type=date]");
            inputDate.Change("");

            var expectedDoom = $@"<h1>Weather forecast</h1>
<p>This component demonstrates fetching data from the server.</p>
<form>
            <div class=""form-group""><label>Date</label>
                <input aria-invalid="""" type=""date"" class=""modified invalid"" value=""{firstItem.Date.ToString("yyyy-MM-dd")}"">
<div class=""validation-message"">The Date field must be a date.</div>
</div>
                <div class=""form-group""><label>Temp. (C)</label>
                <input step=""any"" type=""number"" class=""valid"" value=""{firstItem.TemperatureC}"">
                </div>
                <div class=""form-group""><label>Summary</label>
                <input class=""valid"" value=""{firstItem.Summary}"">
                </div>
                <button type=""button"">Go Back</button></form>";

            component.MarkupMatches(expectedDoom);
        }
    }
}
