using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorConf2021.Server.Service;
using Xunit;

namespace Test
{
    public class WeatherForecastServiceTest
    {
        [Fact]
        public void Get_ReturnData()
        {
            var service = new WeatherForecastService();

            var result = service.Get();

            Assert.NotEmpty(result);
            Assert.Equal(5, result.Length);
        }
        
    }
}
