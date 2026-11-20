// ⋯ (unchanged: the analysis in RunAnalysisAsync)
    // Store the incidents in the SQLite database and read them back
    Console.WriteLine("== Database ==");
    string connectionString = configuration.GetConnectionString("SecLog")!;
    DbContextOptions<SecLogDbContext> dbOptions =
        new DbContextOptionsBuilder<SecLogDbContext>()
            .UseSqlite(connectionString)
            .Options;
    using (SecLogDbContext db = new(dbOptions))
    {
        db.Database.EnsureCreated();
        db.Incidents.AddRange(incidents.Select(StoredIncident.From));
        db.SaveChanges();
        List<StoredIncident> stored = db.Incidents.OrderBy(i => i.Id).ToList();
        Console.WriteLine($"Stored incidents: {stored.Count}");
        foreach (StoredIncident row in stored)
        {
            Console.WriteLine(
                $"#{row.Id} {row.User}: {row.FailureCount} failed logins, {row.Status} ({row.Source})");
        }
    }
}
