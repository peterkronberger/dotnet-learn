using System.Text.Json;

var web = new JsonSerializerOptions(JsonSerializerDefaults.Web);

// 1. A missing property keeps the default value
string partial = """{"id": 3, "user": "dave"}""";
var a = JsonSerializer.Deserialize<Ticket>(partial, web);
Console.WriteLine($"1: {a}");

// 2. null in a non-nullable member is accepted by default
string withNull = """{"id": 4, "user": null, "note": null}""";
var b = JsonSerializer.Deserialize<Ticket>(withNull, web);
Console.WriteLine($"2: user is null: {b!.User is null}");

// 3. The option RespectNullableAnnotations rejects it
var strict = new JsonSerializerOptions(JsonSerializerDefaults.Web)
{
    RespectNullableAnnotations = true
};
try
{
    JsonSerializer.Deserialize<Ticket>(withNull, strict);
}
catch (JsonException)
{
    Console.WriteLine("3: JsonException for user = null");
}

// 4. A required member must be present
try
{
    JsonSerializer.Deserialize<Account>("""{"name": "x"}""", web);
}
catch (JsonException)
{
    Console.WriteLine("4: JsonException, id is missing");
}

// 5. Text that is not valid JSON
try
{
    JsonSerializer.Deserialize<Ticket>("{id: 3}", web);
}
catch (JsonException)
{
    Console.WriteLine("5: JsonException, invalid JSON");
}

class Ticket
{
    public int Id { get; set; }
    public string User { get; set; } = "unknown";
    public string? Note { get; set; }
    public override string ToString() => $"{Id} {User} {Note ?? "-"}";
}

class Account
{
    public required int Id { get; set; }
    public string Name { get; set; } = "";
}
