    // ⋯ (unchanged: create script, SaveChanges, rows of the incidents)
    // Count the stored events per incident: the database does the counting
    var eventCounts = db.Incidents
        .OrderBy(i => i.Id)
        .Select(i => new { i.User, Count = i.Events.Count })
        .ToList();
    foreach (var eventCount in eventCounts)
    {
        Console.WriteLine($"Stored events of {eventCount.User}: {eventCount.Count}");
    }
