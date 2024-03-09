using Newtonsoft.Json.Linq;

namespace _05.LINQ_To_JSON_From_String
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            

            string jsonString = File.ReadAllText(Path.Combine(Environment.CurrentDirectory) + "/../../../People.json");

            var person = JObject.Parse(jsonString);

                Console.WriteLine(person["firstName"]);
                Console.WriteLine(person["lastName"]);

            
        }
    }
}
