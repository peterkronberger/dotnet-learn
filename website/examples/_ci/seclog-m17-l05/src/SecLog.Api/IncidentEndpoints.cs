using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SecLog.Data;

namespace SecLog.Api;

public static class IncidentEndpoints
{
    public static RouteGroupBuilder MapIncidentEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/incidents");

        group.MapGet("/", GetAll);
        group.MapGet("/{id:int}", GetById);
        group.MapPut("/{id:int}/status", ChangeStatus);

        return group;
    }

    private static async Task<List<IncidentSummary>> GetAll(
        SecLogDbContext db, [MaxLength(64)] string? user = null)
    {
        var query = db.Incidents.AsNoTracking();
        if (user is not null)
        {
            query = query.Where(IncidentFilters.ForUser(user));
        }

        var incidents = await query.OrderBy(i => i.Id).ToListAsync();
        return incidents.Select(IncidentSummary.From).ToList();
    }

    private static async Task<IncidentSummary?> GetById(
        int id, SecLogDbContext db)
    {
        var incident = await db.Incidents
            .AsNoTracking()
            .OrderBy(i => i.Id)
            .FirstOrDefaultAsync(i => i.Id == id);
        return incident is null ? null : IncidentSummary.From(incident);
    }

    private static async Task<IncidentSummary?> ChangeStatus(
        int id, StatusChange change, SecLogDbContext db)
    {
        var incident = await db.Incidents.FindAsync(id);
        if (incident is null)
        {
            return null;
        }

        incident.Status = change.Status;
        await db.SaveChangesAsync();
        return IncidentSummary.From(incident);
    }
}
