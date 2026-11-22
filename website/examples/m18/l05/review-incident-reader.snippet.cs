using Microsoft.EntityFrameworkCore;

public class IncidentReader
{
    private static SecLogDbContext db = new();

    public async Task<List<StoredIncident>> ReadForUserAsync(string user)
    {
        var incidents = db.Incidents
            .FromSqlRaw("SELECT * FROM Incidents WHERE User = '" + user + "'")
            .ToList();

        foreach (var incident in incidents)
        {
            incident.Events = db.Events
                .Where(e => e.StoredIncidentId == incident.Id)
                .ToList();
        }

        await db.SaveChangesAsync();
        return incidents;
    }
}
