using Newtonsoft.Json.Linq;

namespace LINQ_To_JSON_JObject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var json = JObject.Parse(@"{'products':[
                {
                    'name': 'Fruits',
                    'products': ['apple', 'banana']
                },
                {
                    'name': 'Vegetables',
                    'products': ['cucumber']
                }
            ]}");
            
            var products = json["products"].Select(t =>
            string.Format("{0} ({1})",
                t["name"],
                string.Join(", ", t["products"])
            ));
            
            // Fruits (apple, banana)
            // Vegetables (cucumber)
        }
    }
}
