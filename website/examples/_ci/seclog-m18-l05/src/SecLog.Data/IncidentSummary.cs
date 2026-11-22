using SecLog.Data.Entities;

namespace SecLog.Data;

/// <summary>The shape of an incident in JSON: the CLI prints it, the API sends it.</summary>
public sealed record IncidentSummary(
    int Id, string User, string Source, string Status, int FailureCount)
{
    /// <summary>Copies the data of a stored incident.</summary>
    public static IncidentSummary From(StoredIncident incident) => new(
        incident.Id,
        incident.User,
        incident.Source,
        incident.Status,
        incident.FailureCount);
}
