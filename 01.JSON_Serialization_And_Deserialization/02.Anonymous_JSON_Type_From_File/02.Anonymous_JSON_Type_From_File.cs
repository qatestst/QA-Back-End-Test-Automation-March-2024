using Newtonsoft.Json;


namespace _02.Anonymous_JSON_Type_From_File
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string jsonStringFromFile = File.ReadAllText(Path.Combine(Environment.CurrentDirectory) + "/../../../Person.JSON");
            
            var template = new
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                JobTitle = string.Empty,
            };

            var person = JsonConvert.DeserializeAnonymousType(jsonStringFromFile, template);
        }
    }
}
