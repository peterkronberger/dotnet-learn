namespace SecLog.Model;

/// <summary>The incident is done.</summary>
/// <param name="Note">A short note about the result.</param>
public sealed record ResolvedStatus(string Note) : IncidentStatus;
