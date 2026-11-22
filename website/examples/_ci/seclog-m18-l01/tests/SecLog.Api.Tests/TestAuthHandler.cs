using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SecLog.Api.Tests;

/// <summary>
/// Replaces the bearer scheme in tests. The request headers choose the
/// user and the role, so no token is needed.
/// </summary>
public class TestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(
        options, logger, encoder)
{
    public const string SchemeName = "Test";
    public const string UserHeader = "X-Test-User";
    public const string RoleHeader = "X-Test-Role";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // No user header: nobody is signed in, the result is "no result"
        if (!Request.Headers.TryGetValue(UserHeader, out var user))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var claims = new List<Claim> { new(ClaimTypes.Name, user.ToString()) };
        if (Request.Headers.TryGetValue(RoleHeader, out var role))
        {
            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
        }

        var identity = new ClaimsIdentity(claims, SchemeName);
        var ticket = new AuthenticationTicket(
            new ClaimsPrincipal(identity), SchemeName);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    /// <summary>Makes the test scheme the default scheme of the app.</summary>
    public static void Replace(IServiceCollection services) =>
        services.AddAuthentication(SchemeName)
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                SchemeName, _ => { });
}

public static class TestAuthExtensions
{
    /// <summary>Sends the test headers with every request of the client.</summary>
    public static HttpClient AsUser(
        this HttpClient client, string user, string? role = null)
    {
        client.DefaultRequestHeaders.Add(TestAuthHandler.UserHeader, user);
        if (role is not null)
        {
            client.DefaultRequestHeaders.Add(TestAuthHandler.RoleHeader, role);
        }

        return client;
    }
}
