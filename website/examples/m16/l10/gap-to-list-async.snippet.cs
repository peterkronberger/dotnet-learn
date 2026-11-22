// Read all incidents without blocking the thread
// and stop when the token is cancelled
List<Incident> incidents = await db.Incidents
    .OrderBy(i => i.Id)
    .___(token);
Console.WriteLine($"Incidents: {incidents.Count}");
