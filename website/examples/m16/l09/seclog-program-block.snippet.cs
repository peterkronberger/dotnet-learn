        // ⋯ (unchanged: store the incidents and print the report)
        // Search the incidents of a user name that comes from outside:
        // FromSql sends the value as a parameter, it never becomes SQL text
        string searchUser = configuration["Search:User"] ?? "";
        var searchResult = db.Incidents
            .FromSql($"SELECT * FROM Incidents WHERE User = {searchUser}")
            .OrderBy(i => i.Id);
        Console.WriteLine(searchResult.ToQueryString());
        foreach (StoredIncident row in searchResult)
        {
            Console.WriteLine($"Found: #{row.Id} {row.User} {row.FailureCount}");
        }
