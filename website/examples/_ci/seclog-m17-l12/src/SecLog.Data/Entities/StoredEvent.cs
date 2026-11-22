using SecLog.Model;

namespace SecLog.Data.Entities;

/// <summary>A failed login as one row of the database.</summary>
public class StoredEvent
{
    /// <summary>The key of the row. The database creates it.</summary>
    public int Id { get; set; }

    public DateTimeOffset Timestamp { get; set; }

    public required string EventType { get; set; }

    public string? User { get; set; }

    /// <summary>The foreign key: the key of the incident of this event.</summary>
    public int StoredIncidentId { get; set; }

    /// <summary>The reference navigation to the incident.</summary>
    public StoredIncident StoredIncident { get; set; } = null!;

    /// <summary>Copies the data of a domain event into a new row.</summary>
    public static StoredEvent From(LogEvent logEvent) => new()
    {
        Timestamp = logEvent.Timestamp,
        EventType = logEvent.EventType,
        User = logEvent.User,
    };
}
