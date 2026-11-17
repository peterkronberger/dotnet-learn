[Fact]
public void IsActive_OneSecondAfterWindowEnd_ReturnsFalse()
{
    var clock = new ___(Start);
    var tracker = new FailureTracker(clock);
    tracker.RecordFailure();

    clock.___(Window + TimeSpan.FromSeconds(1));

    Assert.False(tracker.IsActive(Window));
}
