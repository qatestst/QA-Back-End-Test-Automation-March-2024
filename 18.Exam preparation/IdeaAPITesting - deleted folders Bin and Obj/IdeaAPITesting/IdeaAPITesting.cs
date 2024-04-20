using IdeaAPITesting.Models;
using RestSharp;
using RestSharp.Authenticators;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace IdeaAPITesting
{
    public class IdeaAPITesting
    {
        private RestClient client;
        private const string BASE_URL = "http://softuni-qa-loadbalancer-2137572849.eu-north-1.elb.amazonaws.com:84";
        private const string EMAIL = "test-pesho@test.bg";
        private const string PASSWORD = "123456";

        private static string lastIdeaId;

        
        [OneTimeSetUp]
        public void Setup()
        {
            string jwtToken = GetJwtToken(EMAIL, PASSWORD);

            var options = new RestClientOptions(BASE_URL)
            {
                Authenticator = new JwtAuthenticator(jwtToken)
            };

            client = new RestClient(options);

        }

        private string GetJwtToken(string email, string password)
        {
            RestClient authClient = new RestClient(BASE_URL);
            var request = new RestRequest("/api/User/Authentication");
            request.AddJsonBody(new
            {
                email, password 
            });

            var response = authClient.Execute(request, Method.Post); // The same is:  var response = authClient.Post(request);

            if (response.StatusCode == HttpStatusCode.OK)
            { 
                var content = JsonSerializer.Deserialize<JsonElement>(response.Content);
                var token = content.GetProperty("accessToken").GetString();
                
                if(string.IsNullOrWhiteSpace(token))
                {
                    throw new InvalidOperationException("Token is null or empty.");
                }

                return token;
            
            }
            else
            {
                throw new InvalidOperationException($"Authentication failed: {response.StatusCode}, {response.Content}");
            }


        }

        [Test, Order(1)]
        public void CreateNewIdea_WithCorrectData_ShouldSucceed()
        {
            //Arrange
            var requestData = new IdeaDTO()
            {
                Title = "Test Title",
                Description = "Test Description",

            };
            
            //Act
            var request = new RestRequest("/api/Idea/Create");
            request.AddJsonBody(requestData);
            
            var response = client.Execute(request, Method.Post);
            var responseData = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(responseData.Msg, Is.EqualTo("Successfully created!"));

        }

        [Test, Order(2)]
        public void GetAllIdeas_ShouldReturnNonEmptyArray()
        {
            //Arrange
           

            //Act
            var request = new RestRequest("/api/Idea/All");
            
            var response = client.Execute(request, Method.Get);
            var responseDataArray = JsonSerializer.Deserialize<ApiResponseDTO[]>(response.Content);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(responseDataArray.Length, Is.GreaterThan(0));

            lastIdeaId = responseDataArray[responseDataArray.Length - 1].IdeaId;
        }

        [Test, Order(3)]
        public void EditIdea_WithCorrectData_ShouldSucceed()
        {
            //Arrange
            var requestData = new IdeaDTO()
            {
                Title = "Edited Test Title",
                Description = "Test Description with edits",

            };

            //Act
            var request = new RestRequest("/api/Idea/Edit");
            request.AddQueryParameter("ideaId", lastIdeaId);
            request.AddJsonBody(requestData);

            var response = client.Execute(request, Method.Put);
            var responseData = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(responseData.Msg, Is.EqualTo("Edited successfully"));

        }

        [Test, Order(4)]
        public void DeleteIdea_ShouldSucceed()
        {
            
            var request = new RestRequest("/api/Idea/Delete");
            request.AddQueryParameter("ideaId", lastIdeaId);
            
            var response = client.Execute(request, Method.Delete);
            
            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content, Does.Contain("The idea is deleted!"));

        }

        [Test, Order(5)]
        public void CreateNewIdea_WithIncorrectData_ShouldFail()
        {
            //Arrange
            var requestData = new IdeaDTO()
            {
                Title = "Test Title"
                
            };

            //Act
            var request = new RestRequest("/api/Idea/Create");
            request.AddJsonBody(requestData);

            var response = client.Execute(request, Method.Post);
            var responseData = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
           
        }

        [Test, Order(6)]
        public void EditIdea_WithInorrectData_ShouldFail()
        {
            //Arrange
            var requestData = new IdeaDTO()
            {
                Title = "Edited Test Title",
                Description = "Description edited",
            };

            //Act
            var request = new RestRequest("/api/Idea/Edit");
            request.AddQueryParameter("ideaId", "1122223333");
            request.AddJsonBody(requestData);

            var response = client.Execute(request, Method.Put);
           
            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Does.Contain("There is no such idea!"));
            
        }

        [Test, Order(7)]
        public void DeleteIdea_WithWrongId_ShouldSucceed()
        {

            var request = new RestRequest("/api/Idea/Delete");
            request.AddQueryParameter("ideaId", "223334445");

            var response = client.Execute(request, Method.Delete);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Does.Contain("There is no such idea!"));

        }

    }
}