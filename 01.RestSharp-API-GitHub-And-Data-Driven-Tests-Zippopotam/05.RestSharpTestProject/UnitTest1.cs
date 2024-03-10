using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace _05.RestSharpTestProject
{
    public class GitHubAPITests
    {
        private RestClient client;

        [SetUp]
        public void Setup()
        {
            var options = new RestClientOptions("https://api.github.com")
            { 
                //MaxTimeout = 3000,
                //Authenticator = new HttpBasicAuthenticator("username", "api-token") //GitHub username and api-token
                Authenticator = new HttpBasicAuthenticator("qatestst", "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")
            };

            this.client = new RestClient(options);
        }

        [Test]
        public void Test_GitHubApiRequest()
        {
            //Arrange
            //var client = new RestClient("https://api.github.com");
            var request = new RestRequest("/repos/testnakov/test-nakov-repo/issues", Method.Get);
            
            //Act
            var response = this.client.Get(request);
            
            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void Test_GitHubApiRequestTimeout1000()
        {
            //Arrange
            var request = new RestRequest("/repos/testnakov/test-nakov-repo/issues", Method.Get);
            request.Timeout = 1000;
            //Act
            var response = this.client.Get(request);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void Test_GitHubApiRequestIssuesEndpoint_MoreValidation()
        {
            //Arrange
            var request = new RestRequest("/repos/testnakov/test-nakov-repo/issues", Method.Get);

            //Act
            var response = client.Execute(request);
            var issuesObjects = JsonConvert.DeserializeObject<List<Issue>>(response.Content); // using Newtonsoft.Json NuGet
            //var issuesObj = System.Text.Json.JsonSerializer.Deserialize<List<Issue>>(response.Content); //the same , but using System.Text.Json Nuget

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            foreach (var issue in issuesObjects)
            {
                Assert.That(issue.id, Is.GreaterThan(0));
                Assert.That(issue.number, Is.GreaterThan(0));
                Assert.That(issue.title, Is.Not.Empty);

            }
        }

       

        [Test]
        public void Test_GitHubApiRequestCreateIssue()
        {
            //Arrange
            string title = "This is a Demo Issue";
            string body = "QA Back-End-Automation Course March 2024";
            
            //Act
            var createdIssue = CreateIssue(title, body);

            //Assert
            Assert.That(createdIssue.id, Is.GreaterThan(0));
            Assert.That(createdIssue.number, Is.GreaterThan(0));
            Assert.That(createdIssue.title, Is.Not.Empty);


        }
        
        private Issue CreateIssue(string title, string body)
        {
            var request = new RestRequest("/repos/qatestst/RestSharp-HTTP-Post-Test/issues");
            request.AddBody(new { body, title });
            var response = this.client.Execute(request, Method.Post);
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);
            return issue;
        }

        [Test]
        public void Test_Edit_Github_Issue()
        {
            var request = new RestRequest("/repos/qatestst/RestSharp-HTTP-Post-Test/issues/4");
            
            request.AddJsonBody(new
            {
                title = "Changing the name of the issue that I created"
            }
            );
            var response = client.Execute(request, Method.Patch);
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(issue.id, Is.GreaterThan(0), "Issue ID should be greater than 0.");
            Assert.That(response.Content, Is.Not.Empty, "The response content should not be empty.");
            Assert.That(issue.number, Is.GreaterThan(0), "Issue number should be greater than 0.");
            Assert.That(issue.title, Is.EqualTo("Changing the name of the issue that I created"));
        }




    }
}