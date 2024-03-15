using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _01.Weather_Forecast
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WeatherForecast forecast = new WeatherForecast()
            {
                Date = DateTime.Now,
                TemperatureC = 32,
                Summary = "New Random Test Summary"
            };

            string weatherInfo = JsonSerializer.Serialize(forecast);
            Console.WriteLine(weatherInfo);

        }
    }
}
