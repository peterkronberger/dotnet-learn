// @fold-start
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
// @fold-end

var builder = WebApplication.CreateBuilder(args);
// @fold-start
builder.WebHost.UseTestServer();
builder.Logging.ClearProviders();

// One open in-memory database for the whole program
using var connection = new SqliteConnection("Data Source=:memory:");
connection.Open();
builder.Services.AddDbContext<SecLogDbContext>(
    options => options.UseSqlite(connection));
builder.Services.AddAuthentication("Header")
    .AddScheme<AuthenticationSchemeOptions, HeaderAuthHandler>(
        "Header", _ => { });
// @fold-end
builder.Services.AddAuthorization();
builder.Services.AddValidation();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

// The group requires a login for every endpoint under /incidents
var incidents = app.MapGroup("/incidents").RequireAuthorization();

incidents.MapPost("/search", async (
    SearchRequest request,
    SecLogDbContext db,
    CancellationToken cancellationToken) =>
{
    // FromSql sends the interpolated value as a parameter
    var found = await db.Incidents
        .FromSql($"SELECT * FROM Incidents WHERE User = {request.User}")
        .ToListAsync(cancellationToken);

    return Results.Ok(found.Select(i => $"#{i.Id} {i.User}"));
});
// @fold-start
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SecLogDbContext>();
    db.Database.EnsureCreated();
    db.Incidents.Add(new StoredIncident { User = "bob" });
    db.SaveChanges();
}
await app.StartAsync();
using var client = app.GetTestClient();
// @fold-end

await Send("no user", null, """{"user":"bob"}""");
await Send("invalid body", "alice", """{"user":""}""");
await Send("valid request", "alice", """{"user":"bob"}""");

await app.StopAsync();
// @fold-start

async Task Send(string title, string? user, string json)
{
    using var request = new HttpRequestMessage(
        HttpMethod.Post, "/incidents/search");
    request.Content = new StringContent(
        json, Encoding.UTF8, "application/json");
    if (user is not null) request.Headers.Add("X-User", user);

    using var response = await client.SendAsync(request);
    Console.WriteLine(
        $"{title}: {(int)response.StatusCode} {response.ReasonPhrase}");

    // Print only stable problem details fields (no traceId)
    var body = await response.Content.ReadAsStringAsync();
    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
    {
        using var document = JsonDocument.Parse(body);
        var root = document.RootElement;
        Console.WriteLine($"  title: {root.GetProperty("title")}");
        foreach (var error in root.GetProperty("errors").EnumerateObject())
            Console.WriteLine($"  {error.Name}: {error.Value[0]}");
    }
    else if (body.Length > 0)
    {
        Console.WriteLine($"  body: {body}");
    }
}

// A teaching scheme: the user comes from a request header.
// A real API validates a signed token here.
class HeaderAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(
        options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("X-User", out var user))
            return Task.FromResult(AuthenticateResult.NoResult());

        var identity = new ClaimsIdentity(
            [new Claim(ClaimTypes.Name, user.ToString())], "Header");
        return Task.FromResult(AuthenticateResult.Success(
            new AuthenticationTicket(
                new ClaimsPrincipal(identity), "Header")));
    }
}

class StoredIncident
{
    public int Id { get; set; }
    public string User { get; set; } = "";
}

class SecLogDbContext(DbContextOptions<SecLogDbContext> options)
    : DbContext(options)
{
    public DbSet<StoredIncident> Incidents => Set<StoredIncident>();
}
// @fold-end

public record SearchRequest(
    [property: Required, StringLength(64)] string User);
