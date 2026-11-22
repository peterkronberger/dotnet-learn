using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;

// The server runs in memory: no network and no port.
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseTestServer();
builder.Logging.ClearProviders();
var app = builder.Build();

var statuses = new Dictionary<int, string> { [1] = "open", [2] = "open" };
var nextId = 3;

app.MapGet("/incidents/{id:int}", (int id) =>
    statuses.TryGetValue(id, out var status)
        ? Results.Ok(new { id, status })
        : Results.NotFound());

app.MapPost("/incidents", (NewIncident body) =>
{
    var id = nextId++;
    statuses[id] = "open";
    return Results.Created($"/incidents/{id}", new { id, status = "open" });
});

app.MapPut("/incidents/{id:int}/status", (int id, StatusChange body) =>
{
    statuses[id] = body.Status;
    return Results.Ok(new { id, status = body.Status });
});

app.MapDelete("/incidents/{id:int}", (int id) =>
    statuses.Remove(id) ? Results.NoContent() : Results.NotFound());

await app.StartAsync();
using var client = app.GetTestClient();

Console.WriteLine("== 1. Read one incident ==");
await Send(client, HttpMethod.Get, "/incidents/1");
Console.WriteLine("== 2. Read an incident that does not exist ==");
await Send(client, HttpMethod.Get, "/incidents/99");
Console.WriteLine("== 3. Create an incident, twice ==");
await Send(client, HttpMethod.Post, "/incidents", """{"user":"bob"}""");
await Send(client, HttpMethod.Post, "/incidents", """{"user":"bob"}""");
Console.WriteLine("== 4. Replace the status, twice ==");
await Send(client, HttpMethod.Put, "/incidents/1/status", """{"status":"closed"}""");
await Send(client, HttpMethod.Put, "/incidents/1/status", """{"status":"closed"}""");
Console.WriteLine("== 5. Delete an incident, twice ==");
await Send(client, HttpMethod.Delete, "/incidents/2");
await Send(client, HttpMethod.Delete, "/incidents/2");
Console.WriteLine("== 6. A method the path does not offer ==");
await Send(client, HttpMethod.Delete, "/incidents");

await app.StopAsync();

// @fold-start
// Sends one request and prints the request line, the headers the lesson
// talks about, the status line and the body.
static async Task Send(
    HttpClient client, HttpMethod method, string path, string? json = null)
{
    using var request = new HttpRequestMessage(method, path);
    request.Headers.Accept.ParseAdd("application/json");
    if (json is not null)
        request.Content = new StringContent(
            json, System.Text.Encoding.UTF8, "application/json");

    Console.WriteLine($"> {method} {path} HTTP/1.1");
    Console.WriteLine($"> Accept: {request.Headers.Accept}");
    if (json is not null)
    {
        Console.WriteLine(
            $"> Content-Type: {request.Content!.Headers.ContentType}");
        Console.WriteLine($"> body: {json}");
    }

    using var response = await client.SendAsync(request);
    Console.WriteLine($"< {(int)response.StatusCode} {response.ReasonPhrase}");
    if (response.Headers.Location is { } location)
        Console.WriteLine($"< Location: {location}");
    if (response.Content.Headers.ContentType is { } type)
        Console.WriteLine($"< Content-Type: {type}");
    var body = await response.Content.ReadAsStringAsync();
    if (body.Length > 0)
        Console.WriteLine($"< body: {body}");
    Console.WriteLine();
}
// @fold-end

record NewIncident(string User);
record StatusChange(string Status);
