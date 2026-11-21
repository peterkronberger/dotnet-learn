using Microsoft.Extensions.Time.Testing;
using SecLog.Model;
using Xunit;

namespace SecLog.Tests;

public class IncidentActivityTests
{
    private static readonly TimeSpan Window = TimeSpan.FromMinutes(10);

    // Failed logins of bob in the sample log: line 2 and line 7
    private static readonly DateTimeOffset FirstFailure =
        new(2026, 3, 2, 8, 1, 40, TimeSpan.Zero);

    private static readonly DateTimeOffset LastFailure =
        new(2026, 3, 2, 8, 6, 48, TimeSpan.Zero);

    private static Incident BobIncident()
    {
        var incident = new Incident("bob") { Source = "test" };
        incident.Add(new LogEvent(FirstFailure, "login-failed", "bob"));
        incident.Add(new LogEvent(LastFailure, "login-failed", "bob"));
        return incident;
    }

    [Fact]
    public void IsActive_NoFailures_ReturnsFalse()
    {
        var clock = new FakeTimeProvider(FirstFailure);
        var activity = new IncidentActivity(clock, Window);

        bool result = activity.IsActive(new Incident("bob") { Source = "test" });

        Assert.False(result);
    }

    [Fact]
    public void IsActive_JustAfterLastFailure_ReturnsTrue()
    {
        var clock = new FakeTimeProvider(LastFailure);
        var activity = new IncidentActivity(clock, Window);

        clock.Advance(TimeSpan.FromSeconds(30));

        Assert.True(activity.IsActive(BobIncident()));
    }

    [Theory]
    [InlineData(599, true)]
    [InlineData(600, true)]
    [InlineData(601, false)]
    public void IsActive_SecondsAfterLastFailure_ReturnsExpected(
        int seconds, bool expected)
    {
        var clock = new FakeTimeProvider(LastFailure);
        var activity = new IncidentActivity(clock, Window);

        clock.Advance(TimeSpan.FromSeconds(seconds));

        Assert.Equal(expected, activity.IsActive(BobIncident()));
    }

    [Fact]
    public void IsActive_ClockCrossesTheBorder_ChangesToFalse()
    {
        var clock = new FakeTimeProvider(LastFailure);
        var activity = new IncidentActivity(clock, Window);
        Incident incident = BobIncident();

        bool before = activity.IsActive(incident);
        clock.Advance(TimeSpan.FromMinutes(11));
        bool after = activity.IsActive(incident);

        Assert.True(before);
        Assert.False(after);
    }
}
