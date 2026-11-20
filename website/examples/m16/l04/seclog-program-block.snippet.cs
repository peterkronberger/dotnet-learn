    // ⋯ (unchanged: create script, SaveChanges, rows and event counts)
    // Ask the database: incidents with enough failures, most failures first
    var severe = db.Incidents
        .Where(i => i.FailureCount >= maxFailures)
        .OrderByDescending(i => i.FailureCount)
        .Select(i => new { i.User, i.FailureCount });
    Console.WriteLine(severe.ToQueryString());
    foreach (var row in severe)
    {
        Console.WriteLine($"Severe: {row.User} {row.FailureCount}");
    }
