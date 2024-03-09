using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSON.NET_Attributes
{
    public class WeatherForecast
    {
        [JsonProperty("Date")]
        public DateTime DateOfCreation { get; set; }
        public int TemperatureC { get; set; }

        [JsonIgnore]
        public string Summary { get; set; }

    }
}
