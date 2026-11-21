        // ⋯ (unchanged: create script, SaveChanges, rows and event counts)
        // Ask the database: critical incidents, most failures first
        // (the filter is an expression tree, so the database filters)
        var severe = db.Incidents
            .Where(IncidentFilters.AtLeastFailures(maxFailures))
            .OrderByDescending(i => i.FailureCount)
            .Select(i => new { i.User, i.FailureCount });
