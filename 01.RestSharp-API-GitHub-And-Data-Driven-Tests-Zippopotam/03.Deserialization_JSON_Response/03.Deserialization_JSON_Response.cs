using RestSharp;
using System.Text.Json;

namespace _03.Deserialization_JSON_Response
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("https://api.github.com");
            
            var request = new RestRequest("/users/softuni/repos", Method.Get);
            var resp = client.Execute(request);
            var repos = JsonSerializer.Deserialize<List<Repo>>(resp.Content);
            
            Console.WriteLine(resp.StatusCode);
            
            foreach (var repo in repos) 
            {
                Console.WriteLine(string.Join(" --> ", repo.id, repo.full_name, repo.html_url));
            }

        }
    }
}
