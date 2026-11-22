public class FailureEndpointTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task Get_UnknownRoute_ReturnsNotFound()
    {
        var token = TestContext.Current.CancellationToken;
        var client = factory.___();

        var response = await client.GetAsync("/nothing", token);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
