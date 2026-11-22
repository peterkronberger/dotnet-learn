// @fold-start
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Options;
// @fold-end

var builder = WebApplication.CreateBuilder(args);
// @fold-start
// The server runs in memory: no network and no port.
builder.WebHost.UseTestServer();
builder.Logging.ClearProviders();
// @fold-end

// The scheme reads the credentials of a request (see HeaderAuthHandler)
builder.Services.AddAuthentication("Header")
    .AddScheme<AuthenticationSchemeOptions, HeaderAuthHandler>(
        "Header", _ => { });

builder.Services.AddAuthorizationBuilder()
    // A named policy: the caller needs the role responder
    .AddPolicy("ChangeStatus", policy => policy.RequireRole("responder"))
    // Endpoints without any rule fall back to this policy
    .SetFallbackPolicy(new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser().Build());

var app = builder.Build();

// Who is calling? Then: may the caller do this?
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/ping", () => "pong").AllowAnonymous();

app.MapGet("/incidents", (HttpContext context) =>
        $"incidents for {context.User.Identity?.Name}")
    .RequireAuthorization();

app.MapPut("/incidents/1/status", () => Results.NoContent())
    .RequireAuthorization("ChangeStatus");

app.MapGet("/notes", () => "notes");
// @fold-start
await app.StartAsync();
using var client = app.GetTestClient();
// @fold-end

await Send(HttpMethod.Get, "/ping", null, null);
await Send(HttpMethod.Get, "/incidents", null, null);
await Send(HttpMethod.Get, "/incidents", "alice", null);
await Send(HttpMethod.Put, "/incidents/1/status", "alice", null);
await Send(HttpMethod.Put, "/incidents/1/status", "dave", "responder");
await Send(HttpMethod.Get, "/notes", null, null);
await Send(HttpMethod.Get, "/notes", "alice", null);

await app.StopAsync();
// @fold-start

async Task Send(HttpMethod method, string path, string? user, string? role)
{
    using var request = new HttpRequestMessage(method, path);
    if (user is not null) request.Headers.Add("X-User", user);
    if (role is not null) request.Headers.Add("X-Role", role);

    using var response = await client.SendAsync(request);
    var who = user is null ? "nobody" : $"{user} ({role ?? "no role"})";
    Console.WriteLine(
        $"{method} {path} as {who}: " +
        $"{(int)response.StatusCode} {response.ReasonPhrase}");
}

// A teaching scheme: user and role come from two request headers.
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

        var claims = new List<Claim> { new(ClaimTypes.Name, user.ToString()) };
        if (Request.Headers.TryGetValue("X-Role", out var role))
            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));

        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Header"));
        return Task.FromResult(AuthenticateResult.Success(
            new AuthenticationTicket(principal, "Header")));
    }
}
// @fold-end
