using Microsoft.Extensions.Logging;

namespace SecLog.Model;

/// <summary>Checks incidents with a detector and raises an event for each match.</summary>
public sealed class IncidentMonitor
{
    private readonly Detector detector;
    private readonly ILogger<IncidentMonitor> logger;

    /// <summary>Creates a monitor that uses a detector.</summary>
    /// <param name="detector">Counts the rules that match an incident.</param>
    /// <param name="logger">Receives a warning for every incident found.</param>
    public IncidentMonitor(Detector detector, ILogger<IncidentMonitor> logger)
    {
        this.detector = detector;
        this.logger = logger;
    }

    /// <summary>Raised for every incident that at least one rule matches.</summary>
    public event EventHandler<IncidentFoundEventArgs>? IncidentFound;

    /// <summary>Checks the incidents and raises the event for each match.</summary>
    /// <param name="items">The incidents to check.</param>
    public void Check(IEnumerable<Incident> items)
    {
        foreach (Incident incident in items)
        {
            int matches = detector.CountMatches(incident);
            if (matches > 0)
            {
                logger.LogWarning("Incident found for {User}", incident.User);
                OnIncidentFound(new IncidentFoundEventArgs(incident, matches));
            }
        }
    }

    /// <summary>Raises the IncidentFound event.</summary>
    /// <param name="e">The data of the event.</param>
    private void OnIncidentFound(IncidentFoundEventArgs e)
    {
        IncidentFound?.Invoke(this, e);
    }
}
