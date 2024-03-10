using RestSharp;
using RestSharp.Authenticators;
using System.Text.Json;
using Newtonsoft.Json;


namespace _06.Data_Driven_NUnit_Tests_Zippopotam.us_API
{
    
    public class Tests
    {
        private RestClient client;

        [SetUp]
        public void Setup()
        {
            var options = new RestClientOptions("https://api.zippopotam.us")
            {
                MaxTimeout = 3000                
            };

            this.client = new RestClient(options);
        }

        [TestCase("BG", "1000", "Sofija")]
        [TestCase("BG", "5000", "Veliko Turnovo")]
        [TestCase("CA", "M5S", "Toronto")]
        [TestCase("GB", "B1", "Birmingham")]
        [TestCase("DE", "01067", "Dresden")]
        public void TestZippopotamus(string countryCode, string zipCode, string expectedPlace)
        {
            //Arrange
            var httpRequest = new RestRequest("https://api.zippopotam.us" + "/" + countryCode + "/" + zipCode, Method.Get);

            //Act
            var httpResponse = client.Execute(httpRequest);
            //var location = new JsonDeserializer().Deserialize<Location>(httpResponse);
            //var jsonObjects = JsonConvert.DeserializeObject<List<Location>>(httpResponse.Content)
            var location = JsonConvert.DeserializeObject<Location>(httpResponse.Content);

            //Assert
            StringAssert.Contains(expectedPlace, location.Places[0].PlaceName);

        }
    }
}