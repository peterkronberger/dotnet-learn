using SecLog.Data;

namespace SecLog.Api;

public static class IncidentEndpoints
{
    public static RouteGroupBuilder MapIncidentEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/incidents");
        group.MapGet("/", GetAll);
        return group;
    }

    private static async Task<IResult> GetAll(
        IIncidentService incidents, CancellationToken cancellationToken,
        string? user = null)
    {
        try
        {
            var all = await incidents.GetAllAsync(user, cancellationToken);
            return TypedResults.Ok(all);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(detail: ex.ToString());
        }
    }
}
