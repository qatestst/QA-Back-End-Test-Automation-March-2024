using Newtonsoft.Json;

namespace _01.Anonymous_JSON_Type;
internal class Program
{
    static void Main(string[] args)
    {
        var json = @"{
                    'firstName': 'Svetlin',
                    'lastName': 'Nakov',
                    'jobTitle': 'Technical Trainer'
            }";
        
        var template = new
        {
            FirstName = string.Empty,
            LastName = string.Empty,    
            JobTitle = string.Empty,
        };

        var person = JsonConvert.DeserializeAnonymousType(json, template);
    }
}
