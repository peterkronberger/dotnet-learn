using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace SecLog.Api.Tests;

public sealed class EndpointRulesTests(SecLogApiFactory factory)
    : IClassFixture<SecLogApiFactory>
{
    [Fact]
    public void IncidentEndpoints_AllRequireAuthorization()
    {
        // Creating a client builds the app, so the endpoints exist
        using var client = factory.CreateClient();

        var endpoints = factory.Services
            .GetRequiredService<EndpointDataSource>()
            .Endpoints
            .OfType<RouteEndpoint>()
            .Where(e => e.RoutePattern.RawText!.StartsWith("/incidents"))
            .ToList();

        // A test that finds nothing would pass without checking anything
        Assert.NotEmpty(endpoints);
        foreach (var endpoint in endpoints)
        {
            Assert.True(
                endpoint.Metadata.GetOrderedMetadata<IAuthorizeData>().Any(),
                $"{endpoint.RoutePattern.RawText} has no authorization");
        }
    }
}
