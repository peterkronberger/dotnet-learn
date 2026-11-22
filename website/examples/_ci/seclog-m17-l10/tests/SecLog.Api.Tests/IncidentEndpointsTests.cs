using System.Net;
using System.Net.Http.Json;
using SecLog.Data;
using Xunit;

namespace SecLog.Api.Tests;

public class IncidentEndpointsTests(SecLogApiFactory factory)
    : IClassFixture<SecLogApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetIncidents_SeededDatabase_ReturnsAll()
    {
        var token = TestContext.Current.CancellationToken;

        var response = await _client.GetAsync("/incidents", token);
        var incidents = await response.Content
            .ReadFromJsonAsync<List<IncidentSummary>>(token);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(incidents);
        Assert.Equal(["bob", "dave"], incidents.Select(i => i.User));
    }

    [Fact]
    public async Task GetIncident_KnownId_ReturnsIncident()
    {
        var token = TestContext.Current.CancellationToken;

        var incident = await _client
            .GetFromJsonAsync<IncidentSummary>("/incidents/1", token);

        Assert.NotNull(incident);
        Assert.Equal(1, incident.Id);
        Assert.Equal("bob", incident.User);
        Assert.Equal(5, incident.FailureCount);
    }

    [Fact]
    public async Task GetIncident_UnknownId_ReturnsNotFound()
    {
        var token = TestContext.Current.CancellationToken;

        var response = await _client.GetAsync("/incidents/99", token);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
