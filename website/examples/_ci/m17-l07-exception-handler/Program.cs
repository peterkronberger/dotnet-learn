// @fold-start
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
// @fold-end

var builder = WebApplication.CreateBuilder(args);
// @fold-start
// The server runs in memory: no network and no port.
builder.WebHost.UseTestServer();
builder.Logging.ClearProviders();
// @fold-end

builder.Services.AddProblemDetails();
// Handlers run in the order they are registered
builder.Services.AddExceptionHandler<IncidentNotFoundHandler>();

var app = builder.Build();

app.UseExceptionHandler();

var incidents = new Dictionary<int, string> { [1] = "bob", [2] = "dave" };

app.MapGet("/incidents/{id:int}", (int id) =>
    incidents.TryGetValue(id, out var user)
        ? user
        : throw new IncidentNotFoundException(id));

app.MapGet("/crash", string () =>
    throw new InvalidOperationException("Cannot open seclog.db"));

// @fold-start
await app.StartAsync();
using var client = app.GetTestClient();
// @fold-end

Console.WriteLine("== 1. A known exception type ==");
await Send(client, HttpMethod.Get, "/incidents/99");
Console.WriteLine("== 2. Any other exception ==");
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

// Maps one exception type to a 404 problem response
class IncidentNotFoundHandler(IProblemDetailsService problemDetails)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
        Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not IncidentNotFoundException notFound)
            return false;

        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        return await problemDetails.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = new ProblemDetails
            {
                Title = "Incident not found",
                Detail = $"There is no incident with id {notFound.Id}.",
            },
        });
    }
}

class IncidentNotFoundException(int id)
    : Exception($"Incident {id} not found")
{
    public int Id { get; } = id;
}
