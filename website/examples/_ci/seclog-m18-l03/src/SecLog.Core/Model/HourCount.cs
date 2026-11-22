namespace SecLog.Model;

/// <summary>The number of failed logins in one hour (UTC).</summary>
/// <param name="Hour">The hour of the day in UTC, from 0 to 23.</param>
/// <param name="Count">The number of failed logins in that hour.</param>
public sealed record HourCount(int Hour, int Count);
