namespace SecLog.Model;

/// <summary>The data of the IncidentFound event.</summary>
sealed class IncidentFoundEventArgs : EventArgs
{
    /// <summary>Creates the event data.</summary>
    /// <param name="incident">The incident that was found.</param>
    /// <param name="matches">The number of rules that match it.</param>
    public IncidentFoundEventArgs(Incident incident, int matches)
    {
        Incident = incident;
        Matches = matches;
    }

    /// <summary>The incident that was found.</summary>
    public Incident Incident { get; }

    /// <summary>The number of rules that match the incident.</summary>
    public int Matches { get; }
}
