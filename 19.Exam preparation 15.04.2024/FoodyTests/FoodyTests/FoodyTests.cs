using FoodyTests.Models;
using RestSharp.Authenticators;
using System.Net;
using System.Text.Json;

namespace FoodyTests
{
    public class FoodyTests
    {
        private RestClient client;
        private static string foodId;

        [OneTimeSetUp]
        public void Setup()
        {
            // Get Auth
            string loginUsername = "test-pesho";
            string loginPassword = "123456";

            string accessToken = GetAccessToken(loginUsername, loginPassword);

            var restOptions = new RestClientOptions("http://softuni-qa-loadbalancer-2137572849.eu-north-1.elb.amazonaws.com:86")
            { 
                Authenticator = new JwtAuthenticator(accessToken),
            };
            this.client = new RestClient(restOptions);
        }

        private string GetAccessToken(string username, string password)
        {
            var authClient = new RestClient("http://softuni-qa-loadbalancer-2137572849.eu-north-1.elb.amazonaws.com:86");

            var authRequest = new RestRequest("/api/User/Authentication");
            authRequest.AddBody(
                new AuthenticationRequest{ 
                Username = username,
                Password = password 
            });

            var response = authClient.Execute(authRequest, Method.Post);
            if (response.IsSuccessStatusCode) 
            {
                var content = JsonSerializer.Deserialize<AuthenticationResponse>(response.Content);
                var accessToken = content.AccessToken;
                return accessToken;
            
            }
            else 
            {
                throw new InvalidOperationException("Authentication failed");
            }
        }
        [Order(1)]
        [Test]
        public void CreateFoodWithRequiredFields_ShoulSucceed()
        {
            //Arrange
            var newFood = new FoodDTO
            {
                Name = "Test",
                Description = "Test",
            };
            var request = new RestRequest("/api/Food/Create", Method.Post);
            request.AddBody(newFood);
            
            //Act
            var response = this.client.Execute(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var data = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);
            foodId = data.FoodId;

        }

        [Order(2)]
        [Test]
        public void EditFoodWithNewTitle_ShoulSucceed()
        {
            //Arrange          
            var request = new RestRequest($"/api/Food/Edit/{foodId}", Method.Patch);
            request.AddBody(new[]
            { 
                new
                { 
                    path = "/name",
                    op = "replace",
                    value = "New Food Title",
                }
            
            });

            //Act
            var response = this.client.Execute(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);
            Assert.That(content.Message, Is.EqualTo("Successfully edited"));

        }

        [Order(3)]
        [Test]
        public void GetAllFood_ShoulReturnArrayOfItems()
        {
            //Arrange          
            var request = new RestRequest($"/api/Food/All", Method.Get);
           
            //Act
            var response = this.client.Execute(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = JsonSerializer.Deserialize<List<ApiResponseDTO>>(response.Content);
            Assert.IsNotEmpty(content);
        }

        [Order(4)]
        [Test]
        public void DeleteFood_ShouldSucceed()
        {
            //Arrange
            
            var request = new RestRequest($"/api/Food/Delete/{foodId} ", Method.Delete);
            
            //Act
            var response = this.client.Execute(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
            var content = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            Assert.That(content.Message, Is.EqualTo("Deleted successfully!"));

        }

        [Order(5)]
        [Test]
        public void CreateFoodWithIncorrectData_ShouldFail()
        {
            //Arrange
            var newFood = new FoodDTO
            {
                
            };
            var request = new RestRequest("/api/Food/Create", Method.Post);
            request.AddBody(newFood);

            //Act
            var response = this.client.Execute(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
         
        }

        [Order(6)]
        [Test]
        public void EditNonWxistingFood_ShouldFail()
        {
            //Arrange          
            var nonExistingId = "12333HB56";
            
            var request = new RestRequest($"/api/Food/Edit/{nonExistingId}", Method.Patch);
            request.AddBody(new[]
            {
                new
                {
                    path = "/name",
                    op = "replace",
                    value = "New Food Title",
                }

            });

            //Act
            var response = this.client.Execute(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

            var content = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);
            Assert.That(content.Message, Is.EqualTo("No food revues..."));

        }

        [Order(7)]
        [Test]
        public void DeleteNonWxistingFood_ShouldFail()
        {
            //Arrange
            var nonExistingFood = "1122nonexisting";
            var request = new RestRequest($"/api/Food/Delete/{nonExistingFood} ", Method.Delete);

            //Act
            var response = this.client.Execute(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

            var content = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            Assert.That(content.Message, Is.EqualTo("Unable to delete this food revue!"));

        }

    }
}