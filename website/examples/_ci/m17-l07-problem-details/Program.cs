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

// Registers the service that writes problem details
builder.Services.AddProblemDetails();

var app = builder.Build();

// An unhandled exception becomes a 500 problem response
app.UseExceptionHandler();
// An empty 4xx or 5xx response gets a problem body
app.UseStatusCodePages();

var incidents = new Dictionary<int, string> { [1] = "bob", [2] = "dave" };

app.MapGet("/incidents/{id:int}", Results<Ok<string>, NotFound> (int id) =>
    incidents.TryGetValue(id, out var user)
        ? TypedResults.Ok(user)
        : TypedResults.NotFound());

app.MapPut("/incidents/{id:int}/status", (int id, string status) =>
{
    var errors = new Dictionary<string, string[]>();
    if (status is not ("open" or "investigating" or "closed"))
        errors["Status"] = ["Status must be open, investigating or closed."];
    return errors.Count > 0
        ? Results.ValidationProblem(errors)
        : Results.NoContent();
});

app.MapGet("/crash", string () =>
    throw new InvalidOperationException(
        "Cannot open seclog.db with password 'hunter2'"));

// @fold-start
await app.StartAsync();
using var client = app.GetTestClient();
// @fold-end

Console.WriteLine("== 1. Unknown id: the empty 404 gets a body ==");
await Send(client, HttpMethod.Get, "/incidents/99");
Console.WriteLine("== 2. Unknown route ==");
await Send(client, HttpMethod.Get, "/nothing");
Console.WriteLine("== 3. Validation errors ==");
await Send(client, HttpMethod.Put, "/incidents/1/status?status=archived");
Console.WriteLine("== 4. An unhandled exception ==");
await Send(client, HttpMethod.Get, "/crash");

await app.StopAsync();

// @fold-start
// Sends a request and prints the request line, the status line,
// the Content-Type header and the body. The body is read as JSON and
// the "traceId" member is left out, because it changes on every request.
static async Task Send(HttpClient client, HttpMethod method, string path)
{
    using var request = new HttpRequestMessage(method, path);
    Console.WriteLine($"> {method} {path}");
    using var response = await client.SendAsync(request);
    Console.WriteLine($"< {(int)response.StatusCode} {response.ReasonPhrase}");
    var contentType = response.Content.Headers.ContentType;
    if (contentType is not null)
        Console.WriteLine($"< Content-Type: {contentType.MediaType}");
    var body = await response.Content.ReadAsStringAsync();
    if (body.Length > 0)
    {
        var json = System.Text.Json.Nodes.JsonNode.Parse(body)!.AsObject();
        json.Remove("traceId");
        Console.WriteLine($"< body: {json.ToJsonString()}");
    }
    Console.WriteLine();
}
// @fold-end
