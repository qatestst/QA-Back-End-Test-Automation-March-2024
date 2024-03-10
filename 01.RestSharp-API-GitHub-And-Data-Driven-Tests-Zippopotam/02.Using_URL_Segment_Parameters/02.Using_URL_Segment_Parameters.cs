
using RestSharp;

namespace _02.Using_URL_Segment_Parameters
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("https://api.github.com");
            var request = new RestRequest("/repos/{user}/{repo}/issues/{id}", Method.Get);
            request.AddUrlSegment("user", "testnakov");
            request.AddUrlSegment("repo", "test-nakov-repo");
            request.AddUrlSegment("id", 1);

            var response = client.Execute(request);

            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Content);
        }
    }
}
