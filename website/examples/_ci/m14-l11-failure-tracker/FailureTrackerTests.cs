using Microsoft.Extensions.Time.Testing;
using Xunit;

namespace Tracking.Tests;

public class FailureTrackerTests
{
    private static readonly TimeSpan Window = TimeSpan.FromMinutes(10);

    // The first failure of the sample log (line 2)
    private static readonly DateTimeOffset Start =
        new(2026, 3, 2, 8, 1, 40, TimeSpan.Zero);

    [Fact]
    public void IsActive_NoFailure_ReturnsFalse()
    {
        var clock = new FakeTimeProvider(Start);
        var tracker = new FailureTracker(clock);

        Assert.False(tracker.IsActive(Window));
    }

    [Fact]
    public void IsActive_FiveMinutesAfterFailure_ReturnsTrue()
    {
        var clock = new FakeTimeProvider(Start);
        var tracker = new FailureTracker(clock);
        tracker.RecordFailure();

        clock.Advance(TimeSpan.FromMinutes(5));

        Assert.True(tracker.IsActive(Window));
    }

    [Fact]
    public void IsActive_ExactlyAtWindowEnd_ReturnsTrue()
    {
        var clock = new FakeTimeProvider(Start);
        var tracker = new FailureTracker(clock);
        tracker.RecordFailure();

        clock.Advance(Window);

        Assert.True(tracker.IsActive(Window));
    }

    [Fact]
    public void IsActive_OneSecondAfterWindowEnd_ReturnsFalse()
    {
        var clock = new FakeTimeProvider(Start);
        var tracker = new FailureTracker(clock);
        tracker.RecordFailure();

        clock.Advance(Window + TimeSpan.FromSeconds(1));

        Assert.False(tracker.IsActive(Window));
    }

    [Fact]
    public void RecordFailure_UsesTheTimeOfTheClock()
    {
        var clock = new FakeTimeProvider(Start);
        var tracker = new FailureTracker(clock);
        tracker.RecordFailure();

        // Move the clock to the last failure of bob (line 7)
        clock.SetUtcNow(new DateTimeOffset(2026, 3, 2, 8, 6, 48, TimeSpan.Zero));
        tracker.RecordFailure();
        clock.Advance(TimeSpan.FromMinutes(9));

        Assert.True(tracker.IsActive(Window));
    }
}
