using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Xunit;

namespace SecLog.Api.Tests;

public class OpenApiTests(SecLogApiFactory factory)
    : IClassFixture<SecLogApiFactory>
{
    // The document exists only in development, so the test says so
    private readonly HttpClient _client = factory
        .WithWebHostBuilder(builder => builder.UseEnvironment("Development"))
        .CreateClient();

    [Fact]
    public async Task Document_ProductionEnvironment_ReturnsNotFound()
    {
        var token = TestContext.Current.CancellationToken;
        using var client = factory
            .WithWebHostBuilder(builder => builder.UseEnvironment("Production"))
            .CreateClient();

        var response = await client.GetAsync("/openapi/v1.json", token);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private async Task<JsonElement> GetDocumentAsync()
    {
        var token = TestContext.Current.CancellationToken;
        var response = await _client.GetAsync("/openapi/v1.json", token);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync(token);
        return JsonDocument.Parse(json).RootElement;
    }

    [Fact]
    public async Task Document_Always_ContainsTheIncidentPaths()
    {
        var document = await GetDocumentAsync();

        var paths = document.GetProperty("paths")
            .EnumerateObject().Select(p => p.Name);

        Assert.Contains("/incidents", paths);
        Assert.Contains("/incidents/{id}", paths);
        Assert.Contains("/incidents/{id}/status", paths);
    }

    [Fact]
    public async Task Document_GetIncident_DescribesTheNotFoundResponse()
    {
        var document = await GetDocumentAsync();

        var responses = document.GetProperty("paths")
            .GetProperty("/incidents/{id}").GetProperty("get")
            .GetProperty("responses");

        Assert.True(responses.TryGetProperty("200", out _));
        Assert.True(responses.TryGetProperty("404", out _));
    }

    [Fact]
    public async Task Document_EveryOperation_HasNameAndSummary()
    {
        var document = await GetDocumentAsync();

        var operations = document.GetProperty("paths").EnumerateObject()
            .SelectMany(p => p.Value.EnumerateObject())
            .Select(o => o.Value)
            .ToList();

        Assert.Equal(3, operations.Count);
        foreach (var operation in operations)
        {
            Assert.True(operation.TryGetProperty("operationId", out _));
            Assert.True(operation.TryGetProperty("summary", out _));
        }
    }
}
