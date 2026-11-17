using Xunit;

namespace Checks.Tests;

public class AsyncTests
{
    private static readonly string[] Lines =
    [
        "2026-03-02T08:01:40Z login-failed user=bob ip=203.0.113.7",
        "2026-03-02T08:03:31Z login-success user=carol ip=198.51.100.23",
        "2026-03-02T08:02:05Z login-failed user=bob ip=203.0.113.7",
    ];

    [Fact]
    public async Task CountAsync_ThreeLines_ReturnsTwo()
    {
        var counter = new FailureCounter();

        int result = await counter.CountAsync(
            Lines, TestContext.Current.CancellationToken);

        Assert.Equal(2, result);
    }

    [Fact]
    public async Task CountAsync_CancelledToken_ThrowsCancellation()
    {
        var counter = new FailureCounter();
        using var source = new CancellationTokenSource();
        source.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => counter.CountAsync(Lines, source.Token));
    }
}
