using SecLog.Model;

namespace SecLog.Data.Entities;

/// <summary>An incident as one row of the database.</summary>
public class StoredIncident
{
    /// <summary>The key of the row. The database creates it.</summary>
    public int Id { get; set; }

    public required string User { get; set; }

    public required string Source { get; set; }

    public required string Status { get; set; }

    public int FailureCount { get; set; }

    /// <summary>The collection navigation: the failed logins of this incident.</summary>
    public List<StoredEvent> Events { get; set; } = [];

    /// <summary>Copies the data of a domain incident into a new row.</summary>
    public static StoredIncident From(Incident incident) => new()
    {
        User = incident.User,
        Source = incident.Source,
        Status = incident.Status,
        FailureCount = incident.FailureCount,
        Events = incident.Failures.Select(StoredEvent.From).ToList(),
    };
}
