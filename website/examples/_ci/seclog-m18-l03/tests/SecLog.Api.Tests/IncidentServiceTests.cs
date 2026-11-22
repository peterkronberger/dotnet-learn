using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace SecLog.Api.Tests;

public class IncidentServiceTests(SecLogApiFactory factory)
    : IClassFixture<SecLogApiFactory>
{
    private static CancellationToken Cancelled()
    {
        using var source = new CancellationTokenSource();
        source.Cancel();
        return source.Token;
    }

    [Fact]
    public async Task GetAllAsync_CancelledToken_ThrowsCancellation()
    {
        // A new scope is what one request gets
        using var scope = factory.Services.CreateScope();
        var service = scope.ServiceProvider
            .GetRequiredService<IIncidentService>();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => service.GetAllAsync(null, Cancelled()));
    }

    [Fact]
    public async Task ChangeStatusAsync_CancelledToken_KeepsStatus()
    {
        var token = TestContext.Current.CancellationToken;
        using (var scope = factory.Services.CreateScope())
        {
            var service = scope.ServiceProvider
                .GetRequiredService<IIncidentService>();
            await Assert.ThrowsAnyAsync<OperationCanceledException>(
                () => service.ChangeStatusAsync(2, "resolved", Cancelled()));
        }

        using var checkScope = factory.Services.CreateScope();
        var incident = await checkScope.ServiceProvider
            .GetRequiredService<IIncidentService>()
            .GetAsync(2, token);

        Assert.NotNull(incident);
        Assert.Equal("investigating", incident.Status);
    }

    [Fact]
    public void Services_TwoScopes_GetDifferentInstances()
    {
        using var first = factory.Services.CreateScope();
        using var second = factory.Services.CreateScope();

        var a = first.ServiceProvider.GetRequiredService<IIncidentService>();
        var b = second.ServiceProvider.GetRequiredService<IIncidentService>();
        var a2 = first.ServiceProvider.GetRequiredService<IIncidentService>();

        Assert.NotSame(a, b);
        Assert.Same(a, a2);
    }
}
