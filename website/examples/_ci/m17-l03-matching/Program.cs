// @fold-start
using Microsoft.AspNetCore.TestHost;
// @fold-end

var builder = WebApplication.CreateBuilder(args);
// @fold-start
// The server runs in memory: no network and no port.
builder.WebHost.UseTestServer();
builder.Logging.ClearProviders();
// @fold-end
var app = builder.Build();

// The general route is registered first, the specific ones after it.
app.MapGet("/incidents/{name}", (string name) => $"by name: {name}");
app.MapGet("/incidents/{id:int}", (int id) => $"by id: {id}");
app.MapGet("/incidents/latest", () => "the latest incident");

// A group shares the prefix /users.
var users = app.MapGroup("/users");
users.MapGet("/", () => "all users");
users.MapGet("/{name:alpha}", (string name) => $"user: {name}");

// @fold-start
await app.StartAsync();
using var client = app.GetTestClient();
// @fold-end

Console.WriteLine("== 1. Which route matches? ==");
await Send(client, "/incidents/latest");
await Send(client, "/incidents/7");
await Send(client, "/incidents/bob");
Console.WriteLine("== 2. Group with a prefix ==");
await Send(client, "/users");
await Send(client, "/users/alice");
await Send(client, "/users/42");

await app.StopAsync();

// @fold-start
// Sends a request and prints the request line, the status line and the body.
static async Task Send(HttpClient client, string path)
{
    Console.WriteLine($"> GET {path}");
    using var response = await client.GetAsync(path);
    Console.WriteLine($"< {(int)response.StatusCode} {response.ReasonPhrase}");
    var body = await response.Content.ReadAsStringAsync();
    if (body.Length > 0)
        Console.WriteLine($"< body: {body}");
    Console.WriteLine();
}
// @fold-end
