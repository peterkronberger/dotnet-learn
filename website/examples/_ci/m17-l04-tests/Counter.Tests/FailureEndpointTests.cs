using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class FailureEndpointTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task Get_KnownUser_ReturnsJsonWithFailures()
    {
        var token = TestContext.Current.CancellationToken;
        var client = factory.CreateClient();

        var response = await client.GetAsync("/failures/bob", token);
        var body = await response.Content
            .ReadFromJsonAsync<FailureCount>(token);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(new FailureCount("bob", 5), body);
    }

    [Fact]
    public async Task Get_WithFakeCounter_UsesTheFake()
    {
        var token = TestContext.Current.CancellationToken;
        var client = factory
            .WithWebHostBuilder(builder => builder.ConfigureTestServices(
                services => services.AddSingleton<IFailureCounter, FakeCounter>()))
            .CreateClient();

        var body = await client
            .GetFromJsonAsync<FailureCount>("/failures/bob", token);

        Assert.Equal(new FailureCount("bob", 42), body);
    }

    [Fact]
    public async Task Get_UnknownRoute_ReturnsNotFound()
    {
        var token = TestContext.Current.CancellationToken;
        var client = factory.CreateClient();

        var response = await client.GetAsync("/nothing", token);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}

public class FakeCounter : IFailureCounter
{
    public int Count(string user) => 42;
}
