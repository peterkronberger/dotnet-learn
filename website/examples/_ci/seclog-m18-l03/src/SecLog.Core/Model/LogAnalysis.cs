namespace SecLog.Model;

/// <summary>Analyses of log events with LINQ.</summary>
public static class LogAnalysis
{
    /// <summary>Failed logins per hour in UTC, ordered by hour.</summary>
    /// <param name="events">The events to analyse.</param>
    /// <exception cref="ArgumentNullException"><paramref name="events"/> is null.</exception>
    public static IReadOnlyList<HourCount> FailuresPerHour(
        IEnumerable<LogEvent> events)
    {
        ArgumentNullException.ThrowIfNull(events);

        return events
            .Where(e => e.IsFailure())
            .GroupBy(e => e.Timestamp.UtcDateTime.Hour)
            .OrderBy(g => g.Key)
            .Select(g => new HourCount(g.Key, g.Count()))
            .ToList();
    }
}
