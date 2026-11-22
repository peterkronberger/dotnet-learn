// @fold-start
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.TestHost;
// @fold-end

var builder = WebApplication.CreateBuilder(args);
// @fold-start
// The server runs in memory: no network and no port.
builder.WebHost.UseTestServer();
builder.Logging.ClearProviders();
// @fold-end

// Registers the services that build the OpenAPI document
builder.Services.AddOpenApi();

var app = builder.Build();

// Serves the document as JSON at /openapi/v1.json
app.MapOpenApi();

app.MapGet("/incidents/{id:int}",
    Results<Ok<Incident>, NotFound> (int id) =>
        id == 1
            ? TypedResults.Ok(new Incident(1, "bob"))
            : TypedResults.NotFound())
    .WithName("GetIncident")
    .WithSummary("Returns one incident");

app.MapPut("/incidents/{id:int}/status",
    Results<NoContent, NotFound> (int id, StatusChange change) =>
        TypedResults.NoContent())
    .WithName("ChangeStatus")
    .WithSummary("Changes the status of an incident");
// @fold-start
await app.StartAsync();
using var client = app.GetTestClient();
// @fold-end

var json = await client.GetStringAsync("/openapi/v1.json");
using var document = JsonDocument.Parse(json);
var root = document.RootElement;

Console.WriteLine($"openapi: {root.GetProperty("openapi").GetString()}");

foreach (var path in root.GetProperty("paths").EnumerateObject())
{
    foreach (var operation in path.Value.EnumerateObject())
    {
        var summary = operation.Value.GetProperty("summary").GetString();
        var name = operation.Value.GetProperty("operationId").GetString();
        var codes = operation.Value.GetProperty("responses")
            .EnumerateObject().Select(r => r.Name);
        Console.WriteLine(
            $"{operation.Name.ToUpper()} {path.Name}");
        Console.WriteLine($"  name: {name}");
        Console.WriteLine($"  summary: {summary}");
        Console.WriteLine($"  responses: {string.Join(", ", codes)}");
    }
}

// A validation attribute becomes a constraint in the schema
var status = root.GetProperty("components").GetProperty("schemas")
    .GetProperty("StatusChange").GetProperty("properties")
    .GetProperty("status");
Console.WriteLine(
    $"StatusChange.status maxLength: {status.GetProperty("maxLength")}");

await app.StopAsync();

record Incident(int Id, string User);

record StatusChange([property: MaxLength(32)] string Status);
