app.MapGet("/incidents", async (
    IIncidentService incidents, ___ cancellationToken) =>
{
    return await incidents.GetAllAsync(null, cancellationToken);
});
