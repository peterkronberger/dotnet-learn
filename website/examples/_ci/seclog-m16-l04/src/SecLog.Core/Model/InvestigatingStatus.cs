namespace SecLog.Model;

/// <summary>An analyst works on the incident.</summary>
/// <param name="Analyst">The role name of the analyst.</param>
public sealed record InvestigatingStatus(string Analyst) : IncidentStatus;
