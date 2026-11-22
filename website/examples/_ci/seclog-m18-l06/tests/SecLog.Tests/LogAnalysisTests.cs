using System.Globalization;
using SecLog.Model;
using Xunit;

namespace SecLog.Tests;

public class LogAnalysisTests
{
    private static LogEvent Event(string timestamp, string eventType)
    {
        DateTimeOffset time =
            DateTimeOffset.Parse(timestamp, CultureInfo.InvariantCulture);
        return new LogEvent(time, eventType, "bob");
    }

    [Fact]
    public void FailuresPerHour_SampleFailures_CountsPerHour()
    {
        LogEvent[] events =
        [
            Event("2026-03-02T08:01:40Z", "login-failed"),
            Event("2026-03-02T08:02:05Z", "login-failed"),
            Event("2026-03-02T08:04:02Z", "login-failed"),
            Event("2026-03-02T08:05:15Z", "login-failed"),
            Event("2026-03-02T08:06:48Z", "login-failed"),
            Event("2026-03-02T08:15:03Z", "login-failed"),
            Event("2026-03-02T09:10:09Z", "login-failed"),
        ];

        IReadOnlyList<HourCount> result = LogAnalysis.FailuresPerHour(events);

        Assert.Equal([new HourCount(8, 6), new HourCount(9, 1)], result);
    }

    [Fact]
    public void FailuresPerHour_OtherOffset_CountsInUtc()
    {
        LogEvent[] events =
            [Event("2026-03-02T09:30:00+01:00", "login-failed")];

        IReadOnlyList<HourCount> result = LogAnalysis.FailuresPerHour(events);

        Assert.Equal([new HourCount(8, 1)], result);
    }

    [Fact]
    public void FailuresPerHour_NoEvents_ReturnsEmptyList()
    {
        IReadOnlyList<HourCount> result = LogAnalysis.FailuresPerHour([]);

        Assert.Empty(result);
    }

    [Fact]
    public void FailuresPerHour_OtherEventTypes_AreNotCounted()
    {
        LogEvent[] events =
        [
            Event("2026-03-02T08:00:12Z", "login-success"),
            Event("2026-03-02T09:30:00Z", "logout"),
        ];

        IReadOnlyList<HourCount> result = LogAnalysis.FailuresPerHour(events);

        Assert.Empty(result);
    }
}
