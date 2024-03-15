using System.Text.Json;

namespace _02.Weather_Forecast_JSON_Deserializer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            string jsonString = File.ReadAllText(Path.Combine(Environment.CurrentDirectory) +"/../../../weatherForecast.json");
            var  weatherForecastObjects = JsonSerializer.Deserialize<List<WeatherForecast>>(jsonString);

            foreach(var weather in weatherForecastObjects) 
            {
                Console.WriteLine(string.Join(", ", weather.DateTime, weather.TemperatureC, weather.Summary));
            }
        }
    }
}
