using System.Text.Json;

var incident = new Incident(7, "bob", "open", 5);

// Object to JSON text
string json = JsonSerializer.Serialize(incident);
Console.WriteLine(json);

// JSON text back to an object
Incident? copy = JsonSerializer.Deserialize<Incident>(json);
Console.WriteLine(copy);
Console.WriteLine(copy == incident);

record Incident(int Id, string User, string Status, int FailureCount);
