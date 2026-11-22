// Print the stored incidents as JSON, in the shape the API will use
Console.WriteLine("== JSON ==");
List<IncidentSummary> summaries = (await db.Incidents
    .AsNoTracking()
    .OrderBy(i => i.Id)
    .ToListAsync(stoppingToken))
    .Select(IncidentSummary.From)
    .ToList();
JsonSerializerOptions jsonOptions =
    new(JsonSerializerDefaults.Web) { WriteIndented = true };
Console.WriteLine(JsonSerializer.Serialize(summaries, jsonOptions));
