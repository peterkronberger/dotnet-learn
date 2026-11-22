namespace SecLog.Model;

/// <summary>Decides if an incident is still active, by the time of its last failure.</summary>
public sealed class IncidentActivity
{
    private readonly TimeProvider clock;
    private readonly TimeSpan window;

    /// <summary>Creates the check with a clock and a window.</summary>
    /// <param name="clock">Tells the current time.</param>
    /// <param name="window">How long after its last failure an incident is active.</param>
    public IncidentActivity(TimeProvider clock, TimeSpan window)
    {
        this.clock = clock;
        this.window = window;
    }

    /// <summary>Checks if the last failure lies at most one window in the past.</summary>
    /// <param name="incident">The incident to check.</param>
    /// <returns><c>false</c> for an incident without any failure.</returns>
    public bool IsActive(Incident incident)
    {
        DateTimeOffset? last = incident.LastFailure;
        if (last is null)
        {
            return false;
        }
        return clock.GetUtcNow() - last.Value <= window;
    }
}
