using SecLog.Data;

namespace SecLog.Api;

/// <summary>How the endpoints reach the stored incidents.</summary>
public interface IIncidentService
{
    Task<IReadOnlyList<IncidentSummary>> GetAllAsync(
        string? user, CancellationToken cancellationToken);

    Task<IncidentSummary?> GetAsync(
        int id, CancellationToken cancellationToken);

    /// <summary>Returns false when the incident does not exist.</summary>
    Task<bool> ChangeStatusAsync(
        int id, string status, CancellationToken cancellationToken);
}
