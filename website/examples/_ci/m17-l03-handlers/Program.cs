using Microsoft.AspNetCore.TestHost;

// The server runs in memory: no network and no port.
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseTestServer();
builder.Logging.ClearProviders();
var app = builder.Build();

var incidents = new List<IncidentDto>
{
    new(1, "bob", 5),
    new(2, "dave", 1),
};

app.MapGet("/", () => "SecLog API");
app.MapGet("/incidents", () => incidents);
app.MapGet("/incidents/{id:int}", (int id) =>
    incidents.FirstOrDefault(i => i.Id == id));
app.MapGet("/users/{name}", (string name) => $"Hello {name}");
app.MapGet("/count", CountIncidents);
app.MapGet("/slow", async () =>
{
    await Task.Delay(10);
    return incidents.Count;
});

await app.StartAsync();
using var client = app.GetTestClient();

Console.WriteLine("== 1. Text ==");
await Send(client, "/");
Console.WriteLine("== 2. A list as JSON ==");
await Send(client, "/incidents");
Console.WriteLine("== 3. Route parameter ==");
await Send(client, "/incidents/2");
await Send(client, "/users/alice");
Console.WriteLine("== 4. Constraint fails ==");
await Send(client, "/incidents/abc");
Console.WriteLine("== 5. Handler returns null ==");
await Send(client, "/incidents/99");
Console.WriteLine("== 6. Named and async handler ==");
await Send(client, "/count");
await Send(client, "/slow");
Console.WriteLine("== 7. No matching route ==");
await Send(client, "/nothing");

await app.StopAsync();

int CountIncidents() => incidents.Count;

// @fold-start
// Sends a GET request and prints the request line, the status line,
// the content type and the body.
static async Task Send(HttpClient client, string path)
{
    Console.WriteLine($"> GET {path}");
    using var response = await client.GetAsync(path);
    Console.WriteLine($"< {(int)response.StatusCode} {response.ReasonPhrase}");
    if (response.Content.Headers.ContentType is { } type)
        Console.WriteLine($"< Content-Type: {type}");
    var body = await response.Content.ReadAsStringAsync();
    if (body.Length > 0)
        Console.WriteLine($"< body: {body}");
    Console.WriteLine();
}
// @fold-end

record IncidentDto(int Id, string User, int FailureCount);
