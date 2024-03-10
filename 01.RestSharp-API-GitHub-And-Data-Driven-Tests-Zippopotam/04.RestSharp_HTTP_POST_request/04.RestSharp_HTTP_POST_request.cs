using RestSharp;
using RestSharp.Authenticators;

namespace _04.RestSharp_HTTP_POST_request
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient(new RestClientOptions("https://api.github.com")
            {
                //Authenticator = new HttpBasicAuthenticator("username", "api-token")
                Authenticator = new HttpBasicAuthenticator("qatestst", "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")
            });

            var request = new RestRequest("/repos/qatestst/RestSharp-HTTP-Post-Test/issues", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            
            //request.AddJsonBody(new { title = "Title", body = "Body" });
            request.AddJsonBody(new { title = "New Test Title", body = "Some test txt for Body" });
            
            var response = client.Execute(request);
            
            Console.WriteLine(response.StatusCode);

        }
    }
}
