namespace SecLog.Model;

/// <summary>A log event in compact form. Equal if all values are equal.</summary>
/// <param name="Timestamp">When the event happened.</param>
/// <param name="Type">The kind of the event.</param>
/// <param name="User">The user of the event, if the line has one.</param>
sealed record SecurityEvent(DateTimeOffset Timestamp, EventType Type, string? User);
