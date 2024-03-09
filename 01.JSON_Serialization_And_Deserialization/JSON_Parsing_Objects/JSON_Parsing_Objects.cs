using JSON.NET_Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO.Pipes;
namespace JSON_Parsing_Objects
{
    internal class JSON_Parsing_Objects
    {
        static void Main(string[] args)
        {
            
            WeatherForecast weatherForecast = new WeatherForecast()
            {
                DateOfCreation = DateTime.Now,
                TemperatureC = 32,
                Summary = "Somne test text for summary"
            };
            
            
            DefaultContractResolver contractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            var serialized = JsonConvert.SerializeObject(weatherForecast, new JsonSerializerSettings()
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented,
            });
        }
    }
}
