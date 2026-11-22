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

        return group;
    }

    private static async Task<List<IncidentSummary>> GetAll(
        SecLogDbContext db)
    {
        var incidents = await db.Incidents
            .AsNoTracking()
            .OrderBy(i => i.Id)
            .ToListAsync();
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
}
