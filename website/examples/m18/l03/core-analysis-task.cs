using System.Globalization;
using SecLog.Model;

// @fold-start
// Build the canonical sample events (timestamps in UTC)
LogEvent[] logEvents =
[
    Make("2026-03-02T08:00:12Z", "login-success", "alice"),
    Make("2026-03-02T08:01:40Z", "login-failed", "bob"),
    Make("2026-03-02T08:02:05Z", "login-failed", "bob"),
    Make("2026-03-02T08:03:31Z", "login-success", "carol"),
    Make("2026-03-02T08:04:02Z", "login-failed", "bob"),
    Make("2026-03-02T08:05:15Z", "login-failed", "bob"),
    Make("2026-03-02T08:06:48Z", "login-failed", "bob"),
    Make("2026-03-02T08:07:20Z", "login-success", "dave"),
    Make("2026-03-02T08:15:03Z", "login-failed", "alice"),
    Make("2026-03-02T08:15:30Z", "login-success", "alice"),
    Make("2026-03-02T08:42:11Z", "logout", "carol"),
    Make("2026-03-02T09:10:09Z", "login-failed", "dave"),
    Make("2026-03-02T09:10:41Z", "login-success", "dave"),
    Make("2026-03-02T09:30:00Z", "logout", "alice"),
];

static LogEvent Make(string timestamp, string eventType, string user)
{
    DateTimeOffset time =
        DateTimeOffset.Parse(timestamp, CultureInfo.InvariantCulture);
    return new LogEvent(time, eventType, user);
}
// @fold-end

// Print the failures per hour
try
{
    foreach (HourCount row in LogAnalysis.FailuresPerHour(logEvents))
    {
        Console.WriteLine($"{row.Hour:D2}:00 {row.Count} failures");
    }
}
catch (NotImplementedException)
{
    Console.WriteLine("LogAnalysis.FailuresPerHour is not implemented yet.");
}

namespace SecLog.Model
{
    // @fold-start
    // Describe one event of the sample log as an object
    public class LogEvent : IEquatable<LogEvent>
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

    /// <summary>Extension methods that ask a log event a question.</summary>
    public static class LogEventExtensions
    {
        /// <summary>Checks whether the event is a failed login.</summary>
        /// <param name="logEvent">The event to check.</param>
        public static bool IsFailure(this LogEvent logEvent)
        {
            return logEvent.EventType == "login-failed";
        }

        /// <summary>Checks whether the event belongs to a user.</summary>
        /// <param name="logEvent">The event to check.</param>
        /// <param name="user">The name of the user.</param>
        public static bool IsFrom(this LogEvent logEvent, string user)
        {
            return logEvent.User == user;
        }
    }
    // @fold-end

    /// <summary>The number of failed logins in one hour (UTC).</summary>
    /// <param name="Hour">The hour of the day in UTC, from 0 to 23.</param>
    /// <param name="Count">The number of failed logins in that hour.</param>
    public sealed record HourCount(int Hour, int Count);

    /// <summary>Analyses of log events with LINQ.</summary>
    public static class LogAnalysis
    {
        /// <summary>Failed logins per hour in UTC, ordered by hour.</summary>
        /// <param name="events">The events to analyse.</param>
        /// <exception cref="ArgumentNullException"><paramref name="events"/> is null.</exception>
        public static IReadOnlyList<HourCount> FailuresPerHour(
            IEnumerable<LogEvent> events)
        {
            // TODO: failures only, group by the hour in UTC, order by hour,
            // project into HourCount, return a finished list
            throw new NotImplementedException();
        }
    }
}
