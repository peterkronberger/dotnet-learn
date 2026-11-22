using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SecLog.Data.Entities;

namespace SecLog.Data;

/// <summary>Reusable filters for queries on the stored incidents.</summary>
public static class IncidentFilters
{
    /// <summary>
    /// Incidents with at least this many failed logins. It is an expression
    /// tree, so EF Core translates it to SQL and the database filters.
    /// </summary>
    public static Expression<Func<StoredIncident, bool>> AtLeastFailures(
        int threshold) => incident => incident.FailureCount >= threshold;

    /// <summary>Incidents of one user.</summary>
    public static Expression<Func<StoredIncident, bool>> ForUser(
        string user) => incident => incident.User == user;

    /// <summary>
    /// The query for the incident list: not tracked, optionally of one user,
    /// ordered by id. The database does the filtering.
    /// </summary>
    public static IQueryable<StoredIncident> Search(
        IQueryable<StoredIncident> incidents, string? user)
    {
        var query = incidents.AsNoTracking();
        if (user is not null)
        {
            query = query.Where(ForUser(user));
        }

        return query.OrderBy(i => i.Id);
    }
}
