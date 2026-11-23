[Fact]
public void IsActive_ExactlyAtWindowEnd_ReturnsTrue()
{
    var clock = new FakeTimeProvider(Start);
    var tracker = new FailureTracker(clock);
    tracker.RecordFailure();

    clock.Advance(Window);

    Assert.True(tracker.IsActive(Window));
}
