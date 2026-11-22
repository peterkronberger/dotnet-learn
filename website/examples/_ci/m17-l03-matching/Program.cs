using Microsoft.AspNetCore.TestHost;

// The server runs in memory: no network and no port.
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseTestServer();
builder.Logging.ClearProviders();
var app = builder.Build();

// The general route is registered first, the specific ones after it.
app.MapGet("/incidents/{name}", (string name) => $"by name: {name}");
app.MapGet("/incidents/{id:int}", (int id) => $"by id: {id}");
app.MapGet("/incidents/latest", () => "the latest incident");

// A group shares the prefix /users.
var users = app.MapGroup("/users");
users.MapGet("/", () => "all users");
users.MapGet("/{name:alpha}", (string name) => $"user: {name}");

await app.StartAsync();
using var client = app.GetTestClient();

Console.WriteLine("== 1. Which route matches? ==");
await Send(client, "/incidents/latest");
await Send(client, "/incidents/7");
await Send(client, "/incidents/bob");
Console.WriteLine("== 2. Group with a prefix ==");
await Send(client, "/users");
await Send(client, "/users/alice");
await Send(client, "/users/42");
Console.WriteLine("== 3. A method the route does not offer ==");
await Send(client, "/users", HttpMethod.Post);

await app.StopAsync();

// @fold-start
// Sends a request and prints the request line, the status line and the body.
static async Task Send(
    HttpClient client, string path, HttpMethod? method = null)
{
    method ??= HttpMethod.Get;
    Console.WriteLine($"> {method} {path}");
    using var request = new HttpRequestMessage(method, path);
    using var response = await client.SendAsync(request);
    Console.WriteLine($"< {(int)response.StatusCode} {response.ReasonPhrase}");
    var body = await response.Content.ReadAsStringAsync();
    if (body.Length > 0)
        Console.WriteLine($"< body: {body}");
    Console.WriteLine();
}
// @fold-end
