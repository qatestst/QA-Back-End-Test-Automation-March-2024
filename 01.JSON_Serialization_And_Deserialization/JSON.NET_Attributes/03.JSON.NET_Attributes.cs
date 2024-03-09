using Newtonsoft.Json;

namespace JSON.NET_Attributes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string jsonString = File.ReadAllText(Path.Combine(Environment.CurrentDirectory) +"/../../../Forecast.json");
           
            var weatherForecastObject = JsonConvert.DeserializeObject<List<WeatherForecast>>(jsonString);
        }
    }
}
