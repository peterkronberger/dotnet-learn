using System.Text.Json;
using System.Text.Json.Serialization;

var incident = new Incident(7, "bob", "open", 5, "203.0.113.7");

// 1. Default options: names are copied from C#
Console.WriteLine(JsonSerializer.Serialize(incident));

// 2. Web defaults: camelCase names, case-insensitive reading
var web = new JsonSerializerOptions(JsonSerializerDefaults.Web)
{
    WriteIndented = true
};
Console.WriteLine(JsonSerializer.Serialize(incident, web));

// 3. Reading with different capitalization
string text = """{"ID": 8, "USER": "carol", "status": "closed"}""";
Console.WriteLine(JsonSerializer.Deserialize<Incident>(text, web));
Console.WriteLine(JsonSerializer.Deserialize<Incident>(text));

record Incident(
    int Id,
    string User,
    string Status,
    int FailureCount,
    [property: JsonPropertyName("sourceIp")] string? Source = null);
