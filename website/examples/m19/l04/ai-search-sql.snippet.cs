using Microsoft.EntityFrameworkCore;
using SecLog.Data.Entities;

namespace SecLog.Data;

public static class IncidentSearch
{
    public static IQueryable<StoredIncident> ByUser(
        SecLogDbContext db, string term)
    {
        return db.Incidents.FromSqlRaw(
            $"SELECT * FROM Incidents WHERE User = '{term}'");
    }
}
