// @fold-start
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.TestHost;
// @fold-end

var builder = WebApplication.CreateBuilder(args);
// @fold-start
// The server runs in memory: no network and no port.
builder.WebHost.UseTestServer();
builder.Logging.ClearProviders();
// @fold-end
var app = builder.Build();

var incidents = new List<IncidentDto>
{
    new(1, "bob", 5),
    new(2, "dave", 1),
};

// Returns null for an unknown id: this is not "not found".
app.MapGet("/null/{id:int}", (int id) =>
    incidents.FirstOrDefault(i => i.Id == id));

// The return type lists every possible result.
app.MapGet("/incidents/{id:int}", GetIncident);

app.MapPost("/incidents", (NewIncident input) =>
{
    var incident = new IncidentDto(
        incidents.Count + 1, input.User, input.FailureCount);
    incidents.Add(incident);
    return TypedResults.Created($"/incidents/{incident.Id}", incident);
});

app.MapDelete("/incidents/{id:int}", DeleteIncident);

Results<Ok<IncidentDto>, NotFound> GetIncident(int id) =>
    incidents.FirstOrDefault(i => i.Id == id) is { } incident
        ? TypedResults.Ok(incident)
        : TypedResults.NotFound();

Results<NoContent, NotFound> DeleteIncident(int id)
{
    var incident = incidents.FirstOrDefault(i => i.Id == id);
    if (incident is null)
        return TypedResults.NotFound();
    incidents.Remove(incident);
    return TypedResults.NoContent();
}

// @fold-start
await app.StartAsync();
using var client = app.GetTestClient();
// @fold-end

Console.WriteLine("== 1. A handler returns null ==");
await Send(client, HttpMethod.Get, "/null/99");
Console.WriteLine("== 2. Ok or NotFound ==");
await Send(client, HttpMethod.Get, "/incidents/1");
await Send(client, HttpMethod.Get, "/incidents/99");
Console.WriteLine("== 3. Created with a Location header ==");
await Send(client, HttpMethod.Post, "/incidents",
    """{"user":"carol","failureCount":2}""");
Console.WriteLine("== 4. NoContent or NotFound ==");
await Send(client, HttpMethod.Delete, "/incidents/3");
await Send(client, HttpMethod.Delete, "/incidents/3");

await app.StopAsync();

Console.WriteLine("== 5. The handler called without a server ==");
var result = GetIncident(99);
Console.WriteLine(result.Result.GetType().Name);

// @fold-start
// Sends a request and prints the request line, the status line,
// the Location header and the body.
static async Task Send(HttpClient client, HttpMethod method, string path,
    string? json = null)
{
    using var request = new HttpRequestMessage(method, path);
    if (json is not null)
        request.Content = new StringContent(
            json, System.Text.Encoding.UTF8, "application/json");
    Console.WriteLine($"> {method} {path}");
    using var response = await client.SendAsync(request);
    Console.WriteLine($"< {(int)response.StatusCode} {response.ReasonPhrase}");
    if (response.Headers.Location is { } location)
        Console.WriteLine($"< Location: {location}");
    var body = await response.Content.ReadAsStringAsync();
    if (body.Length > 0)
        Console.WriteLine($"< body: {body}");
    Console.WriteLine();
}
// @fold-end

record IncidentDto(int Id, string User, int FailureCount);

record NewIncident(string User, int FailureCount);
