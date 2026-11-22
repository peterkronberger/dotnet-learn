namespace SecLog.Model;

/// <summary>One state of an incident. A with expression makes the next.</summary>
/// <param name="Number">The number of the incident.</param>
/// <param name="User">The name of the user.</param>
/// <param name="FailureCount">The number of failed logins.</param>
/// <param name="Severity">The severity at this state.</param>
/// <param name="Status">The status: open, investigating or closed.</param>
public sealed record IncidentState(int Number, string User, int FailureCount,
    Severity Severity, string Status);
