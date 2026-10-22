using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;

// Demo for spike S2: starts a minimal API in memory (no network) and prints a request and its response.
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseTestServer();
builder.Logging.ClearProviders();

var app = builder.Build();
app.MapGet("/incidents/{id:int}", (int id) => Results.Ok(new { id, title = "Brute force" }));
await app.StartAsync();

using var client = app.GetTestClient();
using var response = await client.GetAsync("/incidents/42");

Console.WriteLine("GET /incidents/42");
Console.WriteLine($"{(int)response.StatusCode} {response.StatusCode}");
Console.WriteLine(response.Content.Headers.ContentType);
Console.WriteLine(await response.Content.ReadAsStringAsync());

await app.StopAsync();
