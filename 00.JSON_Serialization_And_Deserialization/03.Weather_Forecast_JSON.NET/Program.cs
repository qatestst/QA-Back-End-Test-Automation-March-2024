using Newtonsoft.Json;

namespace _03.Weather_Forecast_JSON.NET_Serializer
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

            string weatherForecastJson = JsonConvert.SerializeObject(forecast, Formatting.Indented);
            Console.WriteLine(weatherForecastJson);
        }
    }
}
