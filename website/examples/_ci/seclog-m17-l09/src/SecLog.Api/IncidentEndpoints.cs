using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
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
        IIncidentService incidents, CancellationToken cancellationToken,
        [MaxLength(64)] string? user = null)
    {
        var all = await incidents.GetAllAsync(user, cancellationToken);
        return all.ToList();
    }

    private static async Task<Results<Ok<IncidentSummary>, NotFound>> GetById(
        int id, IIncidentService incidents,
        CancellationToken cancellationToken)
    {
        var incident = await incidents.GetAsync(id, cancellationToken);
        if (incident is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(incident);
    }

    private static async Task<Results<NoContent, NotFound>> ChangeStatus(
        int id, StatusChange change, IIncidentService incidents,
        CancellationToken cancellationToken)
    {
        var found = await incidents.ChangeStatusAsync(
            id, change.Status, cancellationToken);
        return found ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}
