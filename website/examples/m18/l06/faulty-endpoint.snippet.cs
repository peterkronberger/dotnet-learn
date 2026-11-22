using Microsoft.EntityFrameworkCore;

// Faulty on purpose: never compiled, not part of SecLog.
app.MapPost("/incidents/search", (
    SearchRequest request,
    SecLogDbContext db) =>
{
    try
    {
        var sql = "SELECT * FROM Incidents WHERE User = '"
            + request.User + "'";

        var found = db.Incidents
            .FromSqlRaw(sql)
            .ToListAsync()
            .Result;

        return Results.Ok(found);
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.ToString());
    }
});

public record SearchRequest(string User);
