// Gap 1: every endpoint of the group requires a login
var incidents = app.MapGroup("/incidents")
    .___1___();

incidents.MapPost("/search", async (
    SearchRequest request,
    SecLogDbContext db,
    ___2___ cancellationToken) =>
{
    // Gap 3: the value goes to the database as a parameter
    var found = await db.Incidents
        .___3___($"SELECT * FROM Incidents WHERE User = {request.User}")
        .ToListAsync(cancellationToken);

    return Results.Ok(found);
});
