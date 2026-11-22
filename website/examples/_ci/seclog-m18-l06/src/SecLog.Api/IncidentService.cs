using Microsoft.EntityFrameworkCore;
using SecLog.Data;

namespace SecLog.Api;

/// <summary>
/// Reads and changes incidents with the request's DbContext. Both live
/// one request long (scoped).
/// </summary>
public sealed class IncidentService(SecLogDbContext db) : IIncidentService
{
    public async Task<IReadOnlyList<IncidentSummary>> GetAllAsync(
        string? user, CancellationToken cancellationToken)
    {
        var incidents = await IncidentFilters
            .Search(db.Incidents, user)
            .ToListAsync(cancellationToken);
        return incidents.Select(IncidentSummary.From).ToList();
    }

    public async Task<IncidentSummary?> GetAsync(
        int id, CancellationToken cancellationToken)
    {
        var incident = await db.Incidents
            .AsNoTracking()
            .OrderBy(i => i.Id)
            .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        return incident is null ? null : IncidentSummary.From(incident);
    }

    public async Task<bool> ChangeStatusAsync(
        int id, string status, CancellationToken cancellationToken)
    {
        var incident = await db.Incidents.FindAsync([id], cancellationToken);
        if (incident is null)
        {
            return false;
        }

        incident.Status = status;
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
