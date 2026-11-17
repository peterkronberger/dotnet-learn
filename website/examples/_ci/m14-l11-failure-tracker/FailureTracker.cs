namespace Tracking;

public class FailureTracker
{
    private readonly TimeProvider clock;
    private DateTimeOffset? lastFailure;

    public FailureTracker(TimeProvider clock)
    {
        this.clock = clock;
    }

    public void RecordFailure()
    {
        lastFailure = clock.GetUtcNow();
    }

    // Active: the last failure is at most `window` ago
    public bool IsActive(TimeSpan window)
    {
        if (lastFailure is null)
        {
            return false;
        }
        return clock.GetUtcNow() - lastFailure.Value <= window;
    }
}
