using System.Globalization;

namespace SecLog.Model;

// Describe one event of the sample log as an object
class LogEvent : IEquatable<LogEvent>
{
    public DateTimeOffset Timestamp { get; }
    public string EventType { get; }
    public string? User { get; }

    public LogEvent(DateTimeOffset timestamp, string eventType, string? user)
    {
        ArgumentException.ThrowIfNullOrEmpty(eventType);
        this.Timestamp = timestamp;
        this.EventType = eventType;
        this.User = user;
    }

    public string Describe()
    {
        string time = Timestamp.ToString(
            "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        return $"{time} {EventType} {User ?? "unknown"}";
    }

    /// <summary>Splits the event into its three values.</summary>
    public void Deconstruct(
        out DateTimeOffset timestamp, out string eventType, out string? user)
    {
        timestamp = Timestamp;
        eventType = EventType;
        user = User;
    }

    /// <summary>Two events are equal if all three values are equal.</summary>
    public bool Equals(LogEvent? other)
    {
        return other is not null &&
            Timestamp == other.Timestamp &&
            string.Equals(EventType, other.EventType,
                StringComparison.Ordinal) &&
            string.Equals(User, other.User, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as LogEvent);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Timestamp, EventType, User);
    }

    public override string ToString()
    {
        return Describe();
    }
}
