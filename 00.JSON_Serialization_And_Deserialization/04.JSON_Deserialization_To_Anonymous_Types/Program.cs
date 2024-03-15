using Newtonsoft.Json;

namespace _04.JSON_Deserialization_To_Anonymous_Types
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string jsonString = File.ReadAllText(Path.Combine(Environment.CurrentDirectory) + "/../../../People.json");

            var person = new
            {
                firstName = string.Empty,
                lastName = string.Empty,
                jobTitle = string.Empty
            };

            
            var personObject = JsonConvert.DeserializeAnonymousType(jsonString, person);
            //Console.WriteLine(personObject.FirstName, personObject.LastName, personObject.JobTitle);
        }
    }
}
