using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace SecLog.Api.Tests;

public class ProblemDetailsTests(SecLogApiFactory factory)
    : IClassFixture<SecLogApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetIncident_UnknownId_ReturnsProblemDetails()
    {
        var token = TestContext.Current.CancellationToken;

        var response = await _client.GetAsync("/incidents/99", token);
        var problem = await response.Content
            .ReadFromJsonAsync<JsonElement>(token);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json",
            response.Content.Headers.ContentType?.MediaType);
        Assert.Equal(404, problem.GetProperty("status").GetInt32());
    }

    [Fact]
    public async Task ChangeStatus_InvalidStatus_ReturnsErrorsForStatus()
    {
        var token = TestContext.Current.CancellationToken;

        var response = await _client.PutAsJsonAsync(
            "/incidents/1/status", new { status = "archived" }, token);
        var problem = await response.Content
            .ReadFromJsonAsync<JsonElement>(token);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json",
            response.Content.Headers.ContentType?.MediaType);
        Assert.True(problem.GetProperty("errors").TryGetProperty(
            "Status", out _));
    }

    [Fact]
    public async Task GetIncidents_DatabaseFails_ReturnsProblemWithoutDetails()
    {
        var token = TestContext.Current.CancellationToken;
        using var broken = new BrokenDatabaseFactory();
        using var client = broken.CreateClient();

        var response = await client.GetAsync("/incidents", token);
        var body = await response.Content.ReadAsStringAsync(token);

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.Equal("application/problem+json",
            response.Content.Headers.ContentType?.MediaType);
        Assert.DoesNotContain("seclog-missing-dir", body);
        Assert.DoesNotContain("Sqlite", body);
    }
}

/// <summary>An API whose database file is in a missing directory.</summary>
internal sealed class BrokenDatabaseFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var path = Path.Combine(
            Path.GetTempPath(), "seclog-missing-dir", "x.db");
        builder.UseSetting("ConnectionStrings:SecLog", $"Data Source={path}");
    }
}
