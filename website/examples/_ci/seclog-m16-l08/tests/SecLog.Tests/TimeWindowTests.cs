using SecLog.Model;
using Xunit;

namespace SecLog.Tests;

public class TimeWindowTests
{
    // The window of the brute force example: 10 minutes from 08:00:00
    private static readonly DateTimeOffset Start =
        new(2026, 3, 2, 8, 0, 0, TimeSpan.Zero);

    private static readonly TimeSpan Length = TimeSpan.FromMinutes(10);

    [Theory]
    [InlineData(-1, false)]
    [InlineData(0, true)]
    [InlineData(1, true)]
    [InlineData(599, true)]
    [InlineData(600, true)]
    [InlineData(601, false)]
    public void Contains_SecondsAfterStart_ReturnsExpected(
        int seconds, bool expected)
    {
        var window = new TimeWindow(Start, Length);

        bool result = window.Contains(Start.AddSeconds(seconds));

        Assert.Equal(expected, result);
    }

    // Times of the sample log: lines 2, 7, 9 and 14
    public static IEnumerable<TheoryDataRow<string, bool>> SampleTimes =>
    [
        new("2026-03-02T08:01:40Z", true),
        new("2026-03-02T08:06:48Z", true),
        new("2026-03-02T08:15:03Z", false),
        new("2026-03-02T09:30:00Z", false),
    ];

    [Theory]
    [MemberData(nameof(SampleTimes))]
    public void Contains_SampleLogTime_ReturnsExpected(
        string timestamp, bool expected)
    {
        var window = new TimeWindow(Start, Length);
        DateTimeOffset time = DateTimeOffset.Parse(
            timestamp, System.Globalization.CultureInfo.InvariantCulture);

        bool result = window.Contains(time);

        Assert.Equal(expected, result);
    }
}
