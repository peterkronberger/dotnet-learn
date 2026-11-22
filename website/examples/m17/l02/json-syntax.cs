using System.Text.Json;

// One JSON object that uses every kind of value
string json = """
{
  "id": 7,
  "user": "bob",
  "resolved": false,
  "score": 2.5,
  "note": null,
  "ips": ["203.0.113.7", "192.0.2.10"],
  "owner": { "name": "carol" }
}
""";

using JsonDocument doc = JsonDocument.Parse(json);
foreach (JsonProperty property in doc.RootElement.EnumerateObject())
{
    Console.WriteLine($"{property.Name}: {property.Value.ValueKind}");
}
