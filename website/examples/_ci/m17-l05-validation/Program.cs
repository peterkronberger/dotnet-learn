// @fold-start
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.TestHost;
// @fold-end

// Two apps with the same endpoint: one without and one with AddValidation.
await using var plain = await StartApp(withValidation: false);
await using var checkedApp = await StartApp(withValidation: true);

var good = """{"user":"bob","failureCount":5}""";
var bad = """{"user":"","failureCount":-1}""";

Console.WriteLine("== 1. Without AddValidation ==");
await Send(plain.Client, "/incidents", bad);
Console.WriteLine("== 2. With AddValidation: valid body ==");
await Send(checkedApp.Client, "/incidents", good);
Console.WriteLine("== 3. With AddValidation: invalid body ==");
await Send(checkedApp.Client, "/incidents", bad);
Console.WriteLine("== 4. With AddValidation: query parameter ==");
await Send(checkedApp.Client, "/search?user=" + new string('x', 65));
Console.WriteLine("== 5. With AddValidation: rule over two properties ==");
await Send(checkedApp.Client, "/incidents",
    """{"user":"bob","failureCount":0,"status":"closed"}""");

// @fold-start
static async Task<Running> StartApp(bool withValidation)
{
    var builder = WebApplication.CreateBuilder();
    builder.WebHost.UseTestServer();
    builder.Logging.ClearProviders();
    // @fold-end
    if (withValidation)
        builder.Services.AddValidation();

    var app = builder.Build();

    app.MapPost("/incidents", (NewIncident incident) =>
        $"accepted {incident.User} ({incident.FailureCount})");
    app.MapGet("/search", ([MaxLength(64)] string? user) =>
        $"searching for {user}");
    // @fold-start
    await app.StartAsync();
    return new Running(app, app.GetTestClient());
}

// Sends a request and prints the request line, the status line and the body.
static async Task Send(HttpClient client, string path, string? json = null)
{
    var method = json is null ? HttpMethod.Get : HttpMethod.Post;
    using var request = new HttpRequestMessage(method, path);
    if (json is not null)
        request.Content = new StringContent(
            json, System.Text.Encoding.UTF8, "application/json");
    var shown = path.Length > 30 ? path[..30] + "..." : path;
    Console.WriteLine($"> {method} {shown}");
    using var response = await client.SendAsync(request);
    Console.WriteLine($"< {(int)response.StatusCode} {response.ReasonPhrase}");
    var body = await response.Content.ReadAsStringAsync();
    if (body.Length > 0)
        Console.WriteLine($"< body: {body}");
    Console.WriteLine();
}

sealed record Running(WebApplication App, HttpClient Client) : IAsyncDisposable
{
    public async ValueTask DisposeAsync()
    {
        Client.Dispose();
        await App.DisposeAsync();
    }
}
// @fold-end

public record NewIncident(
    [property: Required, StringLength(64)] string User,
    [property: Range(0, 1000)] int FailureCount,
    [property: AllowedValues("open", "investigating", "closed")]
    string Status = "open") : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (Status == "closed" && FailureCount == 0)
            yield return new ValidationResult(
                "A closed incident needs at least one failed login.",
                [nameof(Status), nameof(FailureCount)]);
    }
}
