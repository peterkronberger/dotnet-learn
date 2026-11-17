using SecLog.Model;
using SecLog.Tests.Doubles;
using Xunit;

namespace SecLog.Tests;

public class LogSourceReaderTests
{
    // Four lines of the sample log: lines 1, 2, 3 and 4
    private static readonly string[] Lines =
    [
        "2026-03-02T08:00:12Z login-success user=alice ip=192.0.2.10",
        "2026-03-02T08:01:40Z login-failed user=bob ip=203.0.113.7",
        "2026-03-02T08:02:05Z login-failed user=bob ip=203.0.113.7",
        "2026-03-02T08:03:31Z login-success user=carol ip=198.51.100.23",
    ];

    [Fact]
    public async Task CountFailuresAsync_SampleLines_ReturnsTwo()
    {
        await using var reader = new LogSourceReader("sample", Lines, new ReportSpy());

        int result = await reader.CountFailuresAsync(
            TestContext.Current.CancellationToken);

        Assert.Equal(2, result);
    }

    [Fact]
    public async Task CountFailuresAsync_CancelledToken_ThrowsCancellation()
    {
        await using var reader = new LogSourceReader("sample", Lines, new ReportSpy());
        using var source = new CancellationTokenSource();
        source.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => reader.CountFailuresAsync(source.Token));
    }

    [Fact]
    public async Task DisposeAsync_Always_WritesClosedMessage()
    {
        var spy = new ReportSpy();
        var reader = new LogSourceReader("sample", Lines, spy);

        await reader.DisposeAsync();

        Assert.Equal(["Reader closed: sample"], spy.Lines);
    }
}
