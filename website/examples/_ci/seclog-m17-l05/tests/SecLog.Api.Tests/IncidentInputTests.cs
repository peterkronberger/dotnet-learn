using System.Net;
using System.Net.Http.Json;
using SecLog.Data;
using Xunit;

namespace SecLog.Api.Tests;

public class IncidentInputTests(SecLogApiFactory factory)
    : IClassFixture<SecLogApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetIncidents_FilterByUser_ReturnsOnlyThatUser()
    {
        var token = TestContext.Current.CancellationToken;

        var incidents = await _client.GetFromJsonAsync<List<IncidentSummary>>(
            "/incidents?user=bob", token);

        Assert.NotNull(incidents);
        var incident = Assert.Single(incidents);
        Assert.Equal("bob", incident.User);
    }

    [Fact]
    public async Task GetIncidents_UserTooLong_ReturnsBadRequest()
    {
        var token = TestContext.Current.CancellationToken;
        var user = new string('x', 65);

        var response = await _client.GetAsync($"/incidents?user={user}", token);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ChangeStatus_ValidStatus_ReturnsUpdatedIncident()
    {
        var token = TestContext.Current.CancellationToken;

        var response = await _client.PutAsJsonAsync(
            "/incidents/2/status", new { status = "closed" }, token);
        var incident = await response.Content
            .ReadFromJsonAsync<IncidentSummary>(token);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(incident);
        Assert.Equal("closed", incident.Status);
    }

    [Theory]
    [InlineData("""{"status":"archived"}""")]
    [InlineData("""{"status":""}""")]
    [InlineData("{}")]
    public async Task ChangeStatus_InvalidStatus_ReturnsBadRequest(string json)
    {
        var token = TestContext.Current.CancellationToken;
        using var content = new StringContent(
            json, System.Text.Encoding.UTF8, "application/json");

        var response = await _client.PutAsync(
            "/incidents/1/status", content, token);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
