// Goal: the failure must still count as active after exactly 10 minutes.
// The test must not take 10 minutes.
[Fact]
public void IsActive_ExactlyAtWindowEnd_ReturnsTrue()
{
    var clock = new FakeTimeProvider(Start);
    var tracker = new FailureTracker(clock);
    tracker.RecordFailure();

    // TODO: let the window pass

    Assert.True(tracker.IsActive(Window));
}
