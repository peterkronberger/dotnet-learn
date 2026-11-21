        // ⋯ (unchanged: everything up to the severe incidents)
        // Load every incident with its failed logins in one query
        var withEvents = db.Incidents
            .Include(i => i.Events)
            .OrderBy(i => i.Id);
        Console.WriteLine(withEvents.ToQueryString());
        foreach (StoredIncident row in withEvents)
        {
            Console.WriteLine($"Failures of {row.User}:");
            foreach (StoredEvent failure in row.Events.OrderBy(e => e.Timestamp))
            {
                Console.WriteLine($"  {failure.Timestamp:yyyy-MM-dd'T'HH:mm:ss'Z'}");
            }
        }
