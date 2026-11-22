// @fold-start
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
// @fold-end

var builder = WebApplication.CreateBuilder(args);
// @fold-start
// The server runs in memory: no network and no port.
builder.WebHost.UseTestServer();
builder.Logging.ClearProviders();
// @fold-end
builder.Services.AddSingleton<Greeter>();
var app = builder.Build();

// Inferred: "id" is in the route template, so it comes from the route.
app.MapGet("/incidents/{id:int}", (int id) => $"id={id}");

// Inferred: "user" is not in the route, so it comes from the query string.
app.MapGet("/incidents", (string? user, int minFailures = 0) =>
    $"user={user ?? "(none)"}, minFailures={minFailures}");

// Explicit: the source is chosen with an attribute.
app.MapGet("/whoami", ([FromHeader(Name = "X-Analyst")] string analyst) =>
    $"analyst={analyst}");

// Inferred: a record is a complex type, so it comes from the JSON body.
app.MapPost("/incidents", (NewIncident incident) =>
    $"new incident for {incident.User} ({incident.Source})");

// Inferred: a registered service comes from dependency injection.
app.MapGet("/hello/{name}", (string name, Greeter greeter) =>
    greeter.Greet(name));

// Explicit: "term" is also a route value, but here it is read from the query.
app.MapGet("/search/{term}", ([FromQuery(Name = "term")] string fromQuery) =>
    $"term={fromQuery}");

// @fold-start
await app.StartAsync();
using var client = app.GetTestClient();
// @fold-end

Console.WriteLine("== 1. Route ==");
await Send(client, HttpMethod.Get, "/incidents/7");
Console.WriteLine("== 2. Query string ==");
await Send(client, HttpMethod.Get, "/incidents?user=bob&minFailures=3");
await Send(client, HttpMethod.Get, "/incidents");
Console.WriteLine("== 3. Header ==");
await Send(client, HttpMethod.Get, "/whoami", headers: ("X-Analyst", "alice"));
Console.WriteLine("== 4. Body ==");
await Send(client, HttpMethod.Post, "/incidents",
    """{"user":"bob","source":"sample log"}""");
Console.WriteLine("== 5. Service ==");
await Send(client, HttpMethod.Get, "/hello/alice");
Console.WriteLine("== 6. Explicit query ==");
await Send(client, HttpMethod.Get, "/search/x?term=bob");
Console.WriteLine("== 7. Binding fails ==");
await Send(client, HttpMethod.Get, "/incidents?minFailures=many");
await Send(client, HttpMethod.Get, "/whoami");

await app.StopAsync();

// @fold-start
// Sends a request and prints the request line, the status line and the body.
static async Task Send(HttpClient client, HttpMethod method, string path,
    string? json = null, (string, string)? headers = null)
{
    using var request = new HttpRequestMessage(method, path);
    if (json is not null)
        request.Content = new StringContent(
            json, System.Text.Encoding.UTF8, "application/json");
    if (headers is var (name, value))
        request.Headers.Add(name, value);
    Console.WriteLine($"> {method} {path}");
    using var response = await client.SendAsync(request);
    Console.WriteLine($"< {(int)response.StatusCode} {response.ReasonPhrase}");
    var body = await response.Content.ReadAsStringAsync();
    if (body.Length > 0)
        Console.WriteLine($"< body: {body}");
    Console.WriteLine();
}
// @fold-end

record NewIncident(string User, string Source);

class Greeter
{
    public string Greet(string name) => $"Hello {name}";
}
