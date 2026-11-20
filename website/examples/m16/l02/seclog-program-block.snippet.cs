    // ⋯ (unchanged: connection string and options)
    using (SecLogDbContext db = new(dbOptions))
    {
        db.Database.EnsureCreated();
        Console.WriteLine(db.Database.GenerateCreateScript());
        db.Incidents.AddRange(incidents.Select(StoredIncident.From));
        // ⋯ (unchanged: SaveChanges, query and output of the rows)
    }
