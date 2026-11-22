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

// Every step writes a line into this list, in the order it runs.
var trace = new List<string>();

app.Use(async (context, next) =>
{
    trace.Add("A before");
    await next(context);
    trace.Add("A after");
});

app.Use(async (context, next) =>
{
    trace.Add("B before");
    if (context.Request.Path == "/blocked")
    {
        // Short circuit: next is not called
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        trace.Add("B stops the request");
        return;
    }
    await next(context);
    trace.Add("B after");
});

app.MapGet("/hello", () =>
{
    trace.Add("endpoint");
    return "hello";
});

// @fold-start
await app.StartAsync();
using var client = app.GetTestClient();
// @fold-end

Console.WriteLine("== 1. A request that reaches the endpoint ==");
await Send(client, "/hello");
Console.WriteLine("== 2. A request that middleware B stops ==");
await Send(client, "/blocked");
Console.WriteLine("== 3. A request that matches no endpoint ==");
await Send(client, "/nothing");

await app.StopAsync();

// @fold-start
// Sends a GET request, prints the status code and then the trace
// of this request.
async Task Send(HttpClient client, string path)
{
    trace.Clear();
    using var response = await client.GetAsync(path);
    Console.WriteLine($"> GET {path}");
    Console.WriteLine($"< {(int)response.StatusCode} {response.ReasonPhrase}");
    foreach (var line in trace)
        Console.WriteLine($"  {line}");
    Console.WriteLine();
}
// @fold-end
