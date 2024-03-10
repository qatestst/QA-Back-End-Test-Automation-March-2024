using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _06.Data_Driven_NUnit_Tests_Zippopotam.us_API
{
    public class Location
    {
        [JsonProperty("post code")]
        public string postCode { get; set; }

        [JsonProperty("country")]
        public string country { get; set; }

        [JsonProperty("country abbreviation")]
        public string CountryAbbreviation { get; set; }

        [JsonProperty("places")]
        public List<Place> Places { get; set; }

    }
}
