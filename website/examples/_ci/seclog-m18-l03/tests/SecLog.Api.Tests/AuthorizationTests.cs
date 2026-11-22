using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace SecLog.Api.Tests;

public class AuthorizationTests(SecLogApiFactory factory)
    : IClassFixture<SecLogApiFactory>
{
    private readonly object _body = new { status = "closed" };

    [Fact]
    public async Task GetIncidents_NoUser_ReturnsUnauthorized()
    {
        var token = TestContext.Current.CancellationToken;
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/incidents", token);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetIncidents_UserWithoutRole_ReturnsOk()
    {
        var token = TestContext.Current.CancellationToken;
        using var client = factory.CreateClient().AsUser("alice");

        var response = await client.GetAsync("/incidents", token);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task ChangeStatus_NoUser_ReturnsUnauthorized()
    {
        var token = TestContext.Current.CancellationToken;
        using var client = factory.CreateClient();

        var response = await client.PutAsJsonAsync(
            "/incidents/1/status", _body, token);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ChangeStatus_UserWithoutRole_ReturnsForbidden()
    {
        var token = TestContext.Current.CancellationToken;
        using var client = factory.CreateClient().AsUser("alice");

        var response = await client.PutAsJsonAsync(
            "/incidents/1/status", _body, token);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task ChangeStatus_Responder_ReturnsNoContent()
    {
        var token = TestContext.Current.CancellationToken;
        using var client = factory.CreateClient()
            .AsUser("dave", "responder");

        var response = await client.PutAsJsonAsync(
            "/incidents/2/status", _body, token);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
