using System.Net;
using Xunit;

namespace SecLog.Api.Tests;

public class SecurityHeaderTests(SecLogApiFactory factory)
    : IClassFixture<SecLogApiFactory>
{
    private readonly HttpClient _client =
        factory.CreateClient().AsUser("alice");

    [Fact]
    public async Task GetIncidents_OkResponse_HasNoSniffHeader()
    {
        var token = TestContext.Current.CancellationToken;

        var response = await _client.GetAsync("/incidents", token);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("nosniff", GetHeader(response));
    }

    [Fact]
    public async Task GetIncident_UnknownId_ProblemResponseHasNoSniffHeader()
    {
        var token = TestContext.Current.CancellationToken;

        var response = await _client.GetAsync("/incidents/99", token);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json",
            response.Content.Headers.ContentType?.MediaType);
        Assert.Equal("nosniff", GetHeader(response));
    }

    [Fact]
    public async Task GetIncidents_DatabaseFails_ErrorResponseHasNoSniffHeader()
    {
        var token = TestContext.Current.CancellationToken;
        using var broken = new BrokenDatabaseFactory();
        using var client = broken.CreateClient().AsUser("alice");

        var response = await client.GetAsync("/incidents", token);

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.Equal("nosniff", GetHeader(response));
    }

    private static string? GetHeader(HttpResponseMessage response) =>
        response.Headers.TryGetValues("X-Content-Type-Options", out var values)
            ? string.Join(",", values)
            : null;
}
