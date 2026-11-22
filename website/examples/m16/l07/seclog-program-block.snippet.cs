        // ⋯ (unchanged: everything up to the failures of each incident)
        // Store a new open incident, then set it to investigating
        db.Incidents.Add(new StoredIncident
        {
            User = "carol",
            Source = "sample log",
            Status = "open",
            FailureCount = 1,
        });
        db.SaveChanges();
        StoredIncident carol = db.Incidents.Single(i => i.User == "carol");
        string before = db.Entry(carol).State.ToString();
        carol.Status = "investigating";
        string changed = db.Entry(carol).State.ToString();
        int written = db.SaveChanges();
        string after = db.Entry(carol).State.ToString();
        Console.WriteLine($"State: {before} → {changed} → {after}");
        Console.WriteLine($"Rows written: {written}");
        // The report only reads, so it needs no tracking
        foreach (StoredIncident row in db.Incidents.AsNoTracking().OrderBy(i => i.Id))
        {
            Console.WriteLine($"Report: #{row.Id} {row.User} {row.Status}");
        }
