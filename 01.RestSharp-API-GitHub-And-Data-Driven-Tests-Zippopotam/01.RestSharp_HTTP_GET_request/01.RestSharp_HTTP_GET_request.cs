using RestSharp;
namespace _01.RestSharp_HTTP_GET_request
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("https://api.github.com");
            var request = new RestRequest("/users/softuni/repos", Method.Get);
            var response = client.Execute(request);
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Content);
        }
    }
}
